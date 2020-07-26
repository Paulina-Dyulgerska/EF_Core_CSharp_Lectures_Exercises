namespace Problem_08_Increase_Minion_Age
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        static void Main()
        {
            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
            sqlConnection.Open();

            var idsForChange = Console.ReadLine().Split().Select(int.Parse).ToList();

            UpdateMinionsAgeAndNames(QueryStrings.updateMinionsByIdQueryString, idsForChange, sqlConnection);
            var result = SelectAllMinionNames(QueryStrings.selectAllMinionsQueryString, sqlConnection);
            Console.WriteLine(result);
        }
        private static void UpdateMinionsAgeAndNames(string queryString, List<int> idsForChange, SqlConnection sqlConnection)
        {
            foreach (var id in idsForChange)
            {
                using var sqlCommand = new SqlCommand(queryString, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
        }
        private static string SelectAllMinionNames(string queryString, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);

            var result = new StringBuilder();

            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var minionName = (string)reader["Name"];
                var minionAge = (int)reader["Age"];
                result.AppendLine($"{minionName} {minionAge}");
            }
            return result.ToString().TrimEnd();
        }
    }
}
