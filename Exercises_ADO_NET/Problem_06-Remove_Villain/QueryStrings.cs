using Microsoft.Data.SqlClient.DataClassification;

namespace Problem_06_Remove_Villain
{
    internal class QueryStrings
    {
        internal const string ConnectionString = @"Server=.\SQLEXPRESS;Initial Catalog=MinionsDB;Integrated Security=true;";
        internal const string selectNameFromVillainsByVillainIdQuerySting = @"SELECT [Name] FROM Villains WHERE Id = @villainId";
        internal const string countMinionsByVillainIdQueryString = @"SELECT COUNT(*) FROM MinionsVillains WHERE VillainId = @villainId";
        internal const string deleteVillainByIdFromMinionsVillains = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
        internal const string deleteVillainByIdFromVillains = @"DELETE FROM Villains WHERE Id = @villainId";
        internal const string deleteVillainById =
                                @"BEGIN TRANSACTION
                                DELETE FROM MinionsVillains 
                                      WHERE VillainId = @villainId
                                DELETE FROM Villains
                                      WHERE Id = @villainId
                                COMMIT";
    }
}
