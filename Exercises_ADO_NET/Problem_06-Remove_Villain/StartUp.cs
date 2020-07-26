namespace Problem_06_Remove_Villain
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

            var villainName = SelectVillainNameById(QueryStrings.selectNameFromVillainsByVillainIdQuerySting, villainId, sqlConnection);
            var countMinionsByVillainId = CountMinionsByVillainId(QueryStrings.countMinionsByVillainIdQueryString, villainId, sqlConnection);
            DeleteVillainByVillainId(villainId, sqlConnection);
            var result = BuildResult(villainName, countMinionsByVillainId);

            Console.WriteLine(result);
        }
        private static string SelectVillainNameById(string queryString, int villainId, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            var result = (string)sqlCommand.ExecuteScalar();

            return result;
        }
        private static int CountMinionsByVillainId(string queryString, int villainId, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            var result = (int)sqlCommand.ExecuteScalar();

            return result;
        }
        private static string DeleteVillainByVillainId(int villainId, SqlConnection sqlConnection)
        {
            using SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
            
            string result;

            try
            {
                using var sqlCommandDeleteVillainMinions = new SqlCommand(QueryStrings.deleteVillainByIdFromMinionsVillains, sqlConnection);
                sqlCommandDeleteVillainMinions.Transaction = sqlTransaction;
                sqlCommandDeleteVillainMinions.Parameters.AddWithValue("@villainId", villainId);
                result = sqlCommandDeleteVillainMinions.ExecuteNonQuery().ToString();

                using var sqlCommandDeleteVillain = new SqlCommand(QueryStrings.deleteVillainByIdFromVillains, sqlConnection);
                sqlCommandDeleteVillain.Transaction = sqlTransaction;
                sqlCommandDeleteVillain.Parameters.AddWithValue("@villainId", villainId);
                sqlCommandDeleteVillain.ExecuteNonQuery();

                sqlTransaction.Commit();

                return result;
            }
            catch (Exception commitException)
            {
                result = commitException.Message;

                try
                {
                    sqlTransaction.Rollback();
                    return result;

                }
                catch (Exception rollbackException)
                {
                    result = rollbackException.Message;
                    return result;
                }
            }
        }
        private static string BuildResult(string villainName, int countMinionsByVillainId)
        {
            var str = new StringBuilder();
            if (villainName == null)
            {
                str.AppendLine("No such villain was found.");
            }
            else
            {
                str.AppendLine($"{villainName} was deleted.");
                str.AppendLine($"{countMinionsByVillainId} minions were released.");
            }

            return str.ToString().TrimEnd();
        }
    }
}
