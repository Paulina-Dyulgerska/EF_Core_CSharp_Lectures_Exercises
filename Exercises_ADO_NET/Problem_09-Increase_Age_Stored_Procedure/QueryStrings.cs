namespace Problem_09_Increase_Age_Stored_Procedure
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectMinionQueryString = @"SELECT Name, Age FROM Minions WHERE Id = @Id;";
        internal const string createProcUpdateMinionAgeByIdQueryString = @"
                                                                CREATE OR ALTER PROCEDURE usp_GetOlder (@Id INT)
                                                                AS
                                                                BEGIN
                                                                UPDATE Minions
                                                                   SET Age += 1
                                                                 WHERE Id = @Id;
                                                                END";
        internal const string useStoredProcQueryString = @"EXEC usp_GetOlder @Id";
    }
}
