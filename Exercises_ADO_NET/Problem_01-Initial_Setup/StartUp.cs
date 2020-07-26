namespace Problem_01_Initial_Setup
{
    using Microsoft.Data.SqlClient;
    using System;
    public class StartUp
    {
        private const string databaseName = "MinionsDB";
        static void Main()
        {
            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
            sqlConnection.Open();

            CreateNewDatabase(QueryStrings.createDatabaseString, databaseName, sqlConnection);

            UseNewDatabase(QueryStrings.useDatabaseString, databaseName, sqlConnection);

            CreateTableInDatabase(QueryStrings.createTableCountriesString, sqlConnection);
            CreateTableInDatabase(QueryStrings.createTableTownsString, sqlConnection);
            CreateTableInDatabase(QueryStrings.createTableMinionsString, sqlConnection);
            CreateTableInDatabase(QueryStrings.createTableEvilnessFactorsString, sqlConnection);
            CreateTableInDatabase(QueryStrings.createTableVillainsString, sqlConnection);
            CreateTableInDatabase(QueryStrings.createTableMinionsVillainsString, sqlConnection);

            InsertDataIntoTable(QueryStrings.insertIntoTableCountriesString, sqlConnection);
            InsertDataIntoTable(QueryStrings.insertIntoTableTownsString, sqlConnection);
            InsertDataIntoTable(QueryStrings.insertIntoTableMinionsString, sqlConnection);
            InsertDataIntoTable(QueryStrings.insertIntoTableEvilnessFactorsString, sqlConnection);
            InsertDataIntoTable(QueryStrings.insertIntoTableVillainsString, sqlConnection);
            InsertDataIntoTable(QueryStrings.insertIntoTableMinionsVillainsString, sqlConnection);

            Console.WriteLine("Done!");
        }
        private static void ExecuteNonQueryCommand(string commandString, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(commandString, sqlConnection);
            sqlCommand.ExecuteNonQuery();
        }
        private static void InsertDataIntoTable(string insertIntoTableString, SqlConnection sqlConnection)
        {
            ExecuteNonQueryCommand(insertIntoTableString, sqlConnection);
        }
        private static void CreateTableInDatabase(string sqlCreateTableCommandString, SqlConnection sqlConnection)
        {
            ExecuteNonQueryCommand(sqlCreateTableCommandString, sqlConnection);
        }
        private static void UseNewDatabase(string useDatabaseString, string databaseName, SqlConnection sqlConnection)
        {
            var useNewDatabaseString = useDatabaseString + databaseName;
            ExecuteNonQueryCommand(useNewDatabaseString, sqlConnection);
        }
        private static void CreateNewDatabase(string createDatabaseString, string databaseName, SqlConnection sqlConnection)
        {
            var createDatabaseQueryString = createDatabaseString + databaseName;
            ExecuteNonQueryCommand(createDatabaseQueryString, sqlConnection);
        }
    }
}
