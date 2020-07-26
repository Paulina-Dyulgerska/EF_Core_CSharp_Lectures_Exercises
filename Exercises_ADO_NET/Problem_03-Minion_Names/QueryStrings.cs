namespace Problem_03_Minion_Names
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string getGetMinionsByVillainIdQueryString =
            @"SELECT m.Name, m.Age
                FROM Villains v 
	            LEFT JOIN MinionsVillains mv ON v.Id = mv.VillainId
                JOIN Minions m ON mv.MinionId = m.Id
               WHERE mv.VillainId = @villainId
            ORDER BY m.Name";
        internal const string getVillainNameQueryString = "SELECT [Name] FROM Villains WHERE Id = @villainId";

    }
}

