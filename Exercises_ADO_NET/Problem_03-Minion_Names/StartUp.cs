namespace Problem_03_Minion_Names
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Text;
    public class StartUp
    {
        static void Main()
        {
            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
            sqlConnection.Open();

            var villainId = int.Parse(Console.ReadLine());

            var villainName = GetVillainNameByGivenId(sqlConnection, 
                                QueryStrings.getVillainNameQueryString, 
                                villainId);

            var minionsByVillainName = GetMinionsByVillainId(sqlConnection, 
                                        QueryStrings.getGetMinionsByVillainIdQueryString, 
                                        villainId);
            
            var result = CreateInfoAboutVillain(villainId, villainName, minionsByVillainName);

            Console.WriteLine(result);
        }
        private static string GetMinionsByVillainId(SqlConnection sqlConnection, string queryString, int villainId)
        {
            using var sqlGetMinionsByVillainIdCommand = new SqlCommand(queryString, sqlConnection);
            sqlGetMinionsByVillainIdCommand.Parameters.AddWithValue("@villainId", villainId);

            var result = new StringBuilder();
            var count = 1;

            using var reader = sqlGetMinionsByVillainIdCommand.ExecuteReader();
            if (!reader.HasRows)
            {
                result.AppendLine("(no minions)");
            }
            else
            {
                while (reader.Read())
                {
                    var minionName = (string)reader["Name"];
                    //var minionName = reader["Name"]?.ToString(); //is the same as upper row.
                    var minionAge = (int)reader["Age"];
                    result.AppendLine($"{count}. {minionName} {minionAge}");
                    count++;
                }
            }

            return result.ToString().TrimEnd();
        }
        private static string GetVillainNameByGivenId(SqlConnection sqlConnection, string queryString, int villainId)
        {
            using var sqlGetVillainNameCommand = new SqlCommand(queryString, sqlConnection);
            sqlGetVillainNameCommand.Parameters.AddWithValue("@villainId", villainId);
            var villainName = (string)sqlGetVillainNameCommand.ExecuteScalar();
            return villainName;
        }
        private static string CreateInfoAboutVillain(int villainId, string villainName, string minionsByVillainName)
        {
            var result = new StringBuilder();

            if (villainName == null)
            {
                result.Append($"No villain with ID {villainId} exists in the database.");
            }
            else
            {
                result.AppendLine($"Villain: {villainName}");
                result.Append(minionsByVillainName);
            }

            return result.ToString();
        }
    }
}
