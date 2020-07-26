namespace Problem_07_Print_All_Minion_Names
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectAllMinionNamesQueryString = @"SELECT Name FROM Minions";
    }
}
