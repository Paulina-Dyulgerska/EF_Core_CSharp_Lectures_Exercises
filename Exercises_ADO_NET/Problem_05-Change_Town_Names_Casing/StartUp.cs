namespace Problem_05_Change_Town_Names_Casing
{
    using Microsoft.Data.SqlClient;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class StartUp
    {
        static void Main()
        {
            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
            sqlConnection.Open();

            var countryName = Console.ReadLine();

            ChangeTownsByCountryName(QueryStrings.updateTownNamesToUpperByCountryNameQueryString,
                                countryName,
                                sqlConnection);
            
            var result = SelectTownsByCountryName(QueryStrings.selectTownsByCountryNameQueryString, countryName, sqlConnection);

            Console.WriteLine(result);
        }
        private static int ChangeTownsByCountryName(string queryString, string countryName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@countryName", countryName);
            var result = sqlCommand.ExecuteNonQuery();

            return result;
        }
        private static string SelectTownsByCountryName(string queryString, string countryName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@countryName", countryName);

            var result = new StringBuilder();

            using var reader = sqlCommand.ExecuteReader();
            if (!reader.HasRows)
            {
                result.Append("No town names were affected.");
            }
            else
            {
                var towns = new List<string>();

                while (reader.Read())
                {
                    var townName = (string)reader["Name"];
                    towns.Add(townName);
                }

                result.AppendLine($"{towns.Count} town names were affected.");
                result.Append("[");
                result.Append(String.Join(", ", towns));
                result.Append("]");
            }

            return result.ToString();
        }
    }
}
