//namespace Problem_06_Remove_Villain
//{
//    using Microsoft.Data.SqlClient;
//    using System;
//    using System.Text;

//    public class Program
//    {
//        static void Main()
//        {
//            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
//            sqlConnection.Open();

//            var villainId = int.Parse(Console.ReadLine());

//            var villainName = SelectVillainNameById(QueryStrings.selectNameFromVillainsByVillainIdQuerySting, villainId, sqlConnection);
//            var countMinionsByVillainId = CountMinionsByVillainId(QueryStrings.countMinionsByVillainIdQueryString, villainId, sqlConnection);
//            DeleteVillainByVillainId(QueryStrings.deleteVillainById, villainId, sqlConnection);
//            var result = BuildResult(villainName, countMinionsByVillainId);

//            Console.WriteLine(result);
//        }
//        private static string SelectVillainNameById(string queryString, int villainId, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
//            var result = (string)sqlCommand.ExecuteScalar();

//            return result;
//        }
//        private static int CountMinionsByVillainId(string queryString, int villainId, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
//            var result = (int)sqlCommand.ExecuteScalar();

//            return result;
//        }
//        private static void DeleteVillainByVillainId(string queryString, int villainId, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
//            sqlCommand.ExecuteNonQuery();
//        }
//        private static string BuildResult(string villainName, int countMinionsByVillainId)
//        {
//            var str = new StringBuilder();
//            if (villainName == null)
//            {
//                str.AppendLine("No such villain was found.");
//            }
//            else
//            {
//                str.AppendLine($"{villainName} was deleted.");
//                str.AppendLine($"{countMinionsByVillainId} minions were released.");
//            }

//            return str.ToString().TrimEnd();
//        }
//    }
//}
