namespace MiniORM
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.SqlClient;
	using System.Linq;
	using System.Reflection;

	public abstract class DbContext
	{
		private readonly DatabaseConnection connection;

		//I find dynamically dbSets and store their properties info in this Dictionary!!
		//The second one is of type Dictionary<Type, PropertyInfo>, where we’re 
		//going to store our DbSet<T> properties, once we discover them. Remember,
		//since we’re writing a framework, which other people are going to be 
		//using, we don’t know what entities/DbSets they will define at 
		//compile-time, so we need to discover that at runtime.
		//Type is for instance DbSet<Town>, property TownName.
		//PropertyInfo is the info about TownName. 
		private readonly Dictionary<Type, PropertyInfo> dbSetProperties;

		internal static readonly Type[] AllowedSqlTypes =
		{
			typeof(string),
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(decimal),
			typeof(bool),
			typeof(DateTime),
		};

		protected DbContext(string connectionString)
		{
			this.connection = new DatabaseConnection(connectionString);

			this.dbSetProperties = this.DiscoverDbSets();

			using (new ConnectionManager(connection))
			{
				this.InitializeDbSets();
			}

			//this method is responsible for all Navigational Properties!!
			this.MapAllRelations();
		}

		public void SaveChanges()
		{
			//take all actual dbSets that I have in the current context:
			object[] dbSets = this.dbSetProperties
				.Select(kvp => kvp.Value.GetValue(this))
				.ToArray();

			foreach (IEnumerable<object> dbSet in dbSets)
			{
				object[] invalidEntities = dbSet
					.Where(entity => !IsObjectValid(entity))
					.ToArray();

				if (invalidEntities.Any())
				{
					throw new InvalidOperationException(
						$"{invalidEntities.Length} Invalid Entities found in {dbSet.GetType().Name}!");
					//Invalid.....found in DbSet<Town> <- DbSet<Town> is the Name.
				}
			}

			using (new ConnectionManager(connection))
			{
				using (SqlTransaction transaction = this.connection.StartTransaction())
				{
					foreach (IEnumerable dbSet in dbSets)
					{
						Type dbSetType = dbSet.GetType()
							.GetGenericArguments() //for class<T1, T2> will return T1 and T2.
							.First();	//T1

						//in varuable of type MehtodInfo I could save a method and invoke it later:
						MethodInfo persistMethod = typeof(DbContext)
							.GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
							.MakeGenericMethod(dbSetType); //make the method from the current generic type, i.e. Persist<Town>. Town is dbSetType!

						try
						{
							persistMethod.Invoke(this, new object[] { dbSet });
						}
						catch (TargetInvocationException tie)
						{
							throw tie.InnerException;
						}
						catch (InvalidOperationException)
						{
							transaction.Rollback();
							throw;
						}
						catch (SqlException)
						{
							transaction.Rollback();
							throw;
						}
					}

					transaction.Commit();
				}
			}
		}

		private void Persist<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class, new()
		{
			string tableName = this.GetTableName(typeof(TEntity));

			string[] columns = this.connection.FetchColumnNames(tableName).ToArray();

			if (dbSet.ChangeTracker.Added.Any())
			{
				this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
			}

			TEntity[] modifiedEntities = dbSet.ChangeTracker.GetModifiedEntities(dbSet).ToArray();
			if (modifiedEntities.Any())
			{
				this.connection.UpdateEntities(modifiedEntities, tableName, columns);
			}

			if (dbSet.ChangeTracker.Removed.Any())
			{
				this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
			}
		}

		private void InitializeDbSets()
		{
			foreach (KeyValuePair<Type, PropertyInfo> dbSet in this.dbSetProperties)
			{
				Type dbSetType = dbSet.Key;
				PropertyInfo dbSetProperty = dbSet.Value;

				MethodInfo populateDbSetGeneric = typeof(DbContext)
					.GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);

				populateDbSetGeneric.Invoke(this, new object[] { dbSetProperty });
			}
		}

		private void PopulateDbSet<TEntity>(PropertyInfo dbSet)
			where TEntity : class, new()
		{
			IEnumerable<TEntity> entities = this.LoadTableEntities<TEntity>();

			DbSet<TEntity> dbSetInstance = new DbSet<TEntity>(entities);
			ReflectionHelper.ReplaceBackingField(this, dbSet.Name, dbSetInstance);
			//backing field is the field that is created behind a property, i.e. DbSet<Town> has a field behind it.
		}

		private void MapAllRelations()
		{
			foreach (var dbSetProperty in this.dbSetProperties)
			{
				Type dbSetType = dbSetProperty.Key;

				MethodInfo mapRelationsGeneric = typeof(DbContext)
					.GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);

				object dbSet = dbSetProperty.Value.GetValue(this);

				mapRelationsGeneric.Invoke(this, new[] { dbSet });
			}
		}

		private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class, new()
		{
			Type entityType = typeof(TEntity);

			MapNavigationProperties(dbSet);

			PropertyInfo[] collections = entityType
				.GetProperties()
				.Where(pi =>
					pi.PropertyType.IsGenericType &&
					pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
				.ToArray();

			foreach (PropertyInfo collection in collections)
			{
				Type collectionType = collection.PropertyType.GenericTypeArguments.First();

				MethodInfo mapCollectionMethod = typeof(DbContext)
					.GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(entityType, collectionType);

				mapCollectionMethod.Invoke(this, new object[] { dbSet, collection });
			}
		}

		private void MapCollection<TDbSet, TCollection>(DbSet<TDbSet> dbSet, PropertyInfo collectionProperty)
			where TDbSet : class, new() where TCollection : class, new()
		{
			Type entityType = typeof(TDbSet);
			Type collectionType = typeof(TCollection);

			PropertyInfo[] primaryKeys = collectionType.GetProperties()
				.Where(pi => pi.HasAttribute<KeyAttribute>())
				.ToArray();

			PropertyInfo primaryKey = primaryKeys.First();
			PropertyInfo foreignKey = entityType.GetProperties()
				.First(pi => pi.HasAttribute<KeyAttribute>());

			bool isManyToMany = primaryKeys.Length >= 2;
			if (isManyToMany)
			{
				primaryKey = collectionType.GetProperties()
					.First(pi => collectionType
									 .GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
									 .PropertyType == entityType);
			}

			DbSet<TCollection> navigationDbSet = (DbSet<TCollection>)this
				.dbSetProperties[collectionType].GetValue(this);

			foreach (TDbSet entity in dbSet)
			{
				object primaryKeyValue = foreignKey.GetValue(entity);

				TCollection[] navigationEntities = navigationDbSet
					.Where(navigationEntity => primaryKey.GetValue(navigationEntity).Equals(primaryKeyValue))
					.ToArray();

				ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);
			}
		}

		private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet) where TEntity : class, new()
		{
			Type entityType = typeof(TEntity);

			PropertyInfo[] foreignKeys = entityType.GetProperties()
				.Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
				.ToArray();

			foreach (PropertyInfo foreignKey in foreignKeys)
			{
				string navigationPropertyName =
					foreignKey.GetCustomAttribute<ForeignKeyAttribute>().Name;
				PropertyInfo navigationProperty = entityType.GetProperty(navigationPropertyName);

				object navigationDbSet = this.dbSetProperties[navigationProperty.PropertyType]
					.GetValue(this);

				PropertyInfo navigationPrimaryKey = navigationProperty.PropertyType.GetProperties()
					.First(pi => pi.HasAttribute<KeyAttribute>());

				foreach (TEntity entity in dbSet)
				{
					object foreignKeyValue = foreignKey.GetValue(entity);

					object navigationPropertyValue = ((IEnumerable<object>)navigationDbSet)
						.First(currentNavigationProperty =>
							navigationPrimaryKey.GetValue(currentNavigationProperty).Equals(foreignKeyValue));

					navigationProperty.SetValue(entity, navigationPropertyValue);
				}
			}
		}

		private static bool IsObjectValid(object e)
		{
			var validationContext = new ValidationContext(e);
			var validationErrors = new List<ValidationResult>();

			bool validationResult =
				Validator.TryValidateObject(e, validationContext, validationErrors, validateAllProperties: true);
			return validationResult;
		}

		private IEnumerable<TEntity> LoadTableEntities<TEntity>()
			where TEntity : class
		{
			Type table = typeof(TEntity);

			string[] columns = GetEntityColumnNames(table);

			string tableName = GetTableName(table);

			TEntity[] fetchedRows = this.connection.FetchResultSet<TEntity>(tableName, columns).ToArray();

			return fetchedRows;
		}

		private string GetTableName(Type tableType)
		{
			string tableName = ((TableAttribute)Attribute.GetCustomAttribute(tableType, typeof(TableAttribute)))?.Name;

			if (tableName == null)
			{
				tableName = this.dbSetProperties[tableType].Name;
			}

			return tableName;
		}

		private Dictionary<Type, PropertyInfo> DiscoverDbSets()
		{
			Dictionary<Type, PropertyInfo> dbSets = this.GetType().GetProperties()
				.Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
				.ToDictionary(pi => pi.PropertyType.GetGenericArguments().First(), pi => pi);

			return dbSets;
		}

		private string[] GetEntityColumnNames(Type table)
		{
			string tableName = this.GetTableName(table);
			IEnumerable<string> dbColumns =
				this.connection.FetchColumnNames(tableName);

			string[] columns = table.GetProperties()
				.Where(pi => dbColumns.Contains(pi.Name) &&
							 !pi.HasAttribute<NotMappedAttribute>() &&
							 AllowedSqlTypes.Contains(pi.PropertyType))
				.Select(pi => pi.Name)
				.ToArray();

			return columns;
		}
	}
}