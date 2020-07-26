namespace Problem_04_Add_Minion
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectFromVillainsByNameQueryString = @"SELECT Id FROM Villains WHERE Name = @villainName";
        internal const string selectFromMinionsByNameQueryString = @"SELECT Id FROM Minions WHERE Name = @minionName";
        internal const string selectFromTownsByNameQueryString = @"SELECT Id FROM Towns WHERE Name = @townName";
        internal const string insertIntoMinionsVillainsQueryString = @"INSERT INTO MinionsVillains(MinionId, VillainId) VALUES(@minionId, @villainId)";
        internal const string insertIntoVillainsQueryString = @"INSERT INTO Villains (Name, EvilnessFactorId) 
                                                                VALUES (@villainName, (SELECT Id FROM EvilnessFactors 
                                                                WHERE [Name] = 'Evil'))";
        internal const string insertIntoMinionsQueryString = @"INSERT INTO Minions(Name, Age, TownId) VALUES(@minionName, @minionAge, @townId)";
        internal const string insertIntoTownsQueryString = @"INSERT INTO Towns(Name) VALUES(@townName)";
        internal const string selectFromMinionsTheLastId = @"SELECT TOP(1) Id FROM Minions ORDER BY Id DESC";
    }
}
