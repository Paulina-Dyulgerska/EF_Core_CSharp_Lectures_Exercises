namespace Problem_02_Villain_Names
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

            var result = GetNumberOfMinionsForEachVillain(QueryStrings.getNumberOfMinionsForEachVillainString, sqlConnection);
            
            Console.WriteLine(result);
        }
        private static string GetNumberOfMinionsForEachVillain(string commandString, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(commandString, sqlConnection);

            var result = new StringBuilder();

            using var reader = sqlCommand.ExecuteReader();
            if (!reader.HasRows)
            {
                result.AppendLine("No vallains with more than 3 minions");
            }
            else
            {
                while (reader.Read())
                {
                    var villainName = (string)reader["Name"];
                    var minionsCount = (int)reader["MinionsCount"];
                    result.AppendLine($"{villainName} - {minionsCount}");
                }
            }

            return result.ToString().TrimEnd();
        }
    }
}
