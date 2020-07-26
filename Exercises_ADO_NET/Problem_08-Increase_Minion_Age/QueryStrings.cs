namespace Problem_08_Increase_Minion_Age
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectAllMinionsQueryString = @"SELECT Name, Age FROM Minions";
        internal const string updateMinionsByIdQueryString = @"UPDATE Minions 
                                                   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), 
                                                   Age += 1
                                                   WHERE Id = @Id";
    }
}
