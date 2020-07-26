namespace Problem_07_Print_All_Minion_Names
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

            var names = SelectAllMinionNames(QueryStrings.selectAllMinionNamesQueryString, sqlConnection);

            //Console.WriteLine(String.Join(", ", names)); //original Order

            var result = BuildPrintingResult(names);

            Console.WriteLine(result);
        }
        private static string BuildPrintingResult(IList<string> names)
        {
            var str = new StringBuilder();

            for (int i = 0; i < names.Count / 2; i++)
            {
                str.AppendLine(names[i]);
                str.AppendLine(names[names.Count - 1 - i]);
            }
            if (names.Count % 2 == 1)
            {
                var middleNameIndex = names.Count / 2;
                str.AppendLine(names[middleNameIndex]);
            }

            return str.ToString();
        }

        private static IList<string> SelectAllMinionNames(string queryString, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            var result = (string)sqlCommand.ExecuteScalar();

            var names = new List<string>();

            using var reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                var minionName = (string)reader["Name"];
                names.Add(minionName);
            }
            return names;
        }
    }
}
