//namespace Problem_09_Increase_Age_Stored_Procedure
//{
//    using Microsoft.Data.SqlClient;
//    using System;
//    using System.Data;
//    using System.Text;

//    public class Program
//    {
//        static void Main()
//        {
//            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
//            sqlConnection.Open();

//            var minionId = int.Parse(Console.ReadLine());

//            CreateStoredProcedureForUpdateMinionAgeById(QueryStrings.createProcUpdateMinionAgeByIdQueryString, sqlConnection);

//            ExecuteStoredProcedureForMinionId(QueryStrings.useStoredProcQueryString, minionId, sqlConnection);

//            var result = SelectMinionNameAndAge(QueryStrings.selectMinionQueryString, minionId, sqlConnection);

//            Console.WriteLine(result);
//        }
//        private static void CreateStoredProcedureForUpdateMinionAgeById(string queryString, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.ExecuteNonQuery();
//        }
//        private static void ExecuteStoredProcedureForMinionId(string queryString, int minionId, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.Parameters.AddWithValue("@Id", minionId);
//            sqlCommand.ExecuteNonQuery();
//        }
//        private static string SelectMinionNameAndAge(string queryString, int minionId, SqlConnection sqlConnection)
//        {
//            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
//            sqlCommand.Parameters.AddWithValue("@Id", minionId);

//            var result = new StringBuilder();

//            using var reader = sqlCommand.ExecuteReader();
//            if (!reader.HasRows)
//            {
//                return "No such minion.";
//            }
//            while (reader.Read())
//            {
//                var minionName = (string)reader["Name"];
//                var minionAge = (int)reader["Age"];
//                result.AppendLine($"{minionName.TrimEnd()} – {minionAge} years old");
//            }
//            return result.ToString().TrimEnd();
//        }
//    }
//}
