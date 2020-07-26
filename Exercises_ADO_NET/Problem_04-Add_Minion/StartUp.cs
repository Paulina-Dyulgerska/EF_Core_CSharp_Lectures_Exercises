namespace Problem_04_Add_Minion
{
    using Microsoft.Data.SqlClient;
    using System;
    public class StartUp
    {
        static void Main()
        {
            using var sqlConnection = new SqlConnection(QueryStrings.ConnectionString);
            sqlConnection.Open();

            var inputMinion = Console.ReadLine().Split();
            
            var minionName = inputMinion[1];
            var minionAge = int.Parse(inputMinion[2]);
            var townName = inputMinion[3];

            var villainName = Console.ReadLine().Split()[1];

            var townId = CheckTownName(QueryStrings.selectFromTownsByNameQueryString, townName, sqlConnection);

            AddMinion(QueryStrings.insertIntoMinionsQueryString, minionName, minionAge, townId, sqlConnection);

            var minionId = TakeLastMinionId(QueryStrings.selectFromMinionsTheLastId, sqlConnection);
             
            var villainId = CheckVillainName(QueryStrings.selectFromVillainsByNameQueryString, villainName, sqlConnection);

            AddMinionToVillain(QueryStrings.insertIntoMinionsVillainsQueryString, villainId, minionId, sqlConnection);

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }
        private static int TakeLastMinionId(string queryString, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            var result = (int)sqlCommand.ExecuteScalar();
            return result;
        }
        private static int CheckTownName(string queryString, string townName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@townName", townName);
            var result = sqlCommand.ExecuteScalar();
            if (result == null)
            {
                AddTown(QueryStrings.insertIntoTownsQueryString, townName, sqlConnection);
                Console.WriteLine($"Town {townName} was added to the database.");
                result = sqlCommand.ExecuteScalar();
            }
            return (int)result;
        }
        private static int CheckVillainName(string queryString, string villainName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainName", villainName);
            var result = sqlCommand.ExecuteScalar();
            if (result == null)
            {
                AddVillain(QueryStrings.insertIntoVillainsQueryString, villainName, sqlConnection);
                Console.WriteLine($"Villain {villainName} was added to the database.");
                result = sqlCommand.ExecuteScalar();
            }
            return (int)result;
        }
        private static void AddTown(string queryString, string townName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue($"@townName", townName);
            sqlCommand.ExecuteNonQuery();
        }
        private static void AddVillain(string queryString, string villainName, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue($"@villainName", villainName);
            sqlCommand.ExecuteNonQuery();
        }
        private static void AddMinion(string queryString, string minionName, int minionAge, int townId, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@minionName", minionName);
            sqlCommand.Parameters.AddWithValue("@minionAge", minionAge);
            sqlCommand.Parameters.AddWithValue("@townId", townId);
            sqlCommand.ExecuteNonQuery();
        }
        private static void AddMinionToVillain(string queryString, int villainId, int minionId, SqlConnection sqlConnection)
        {
            using var sqlCommand = new SqlCommand(queryString, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@villainId", villainId);
            sqlCommand.Parameters.AddWithValue("@minionId", minionId);
            sqlCommand.ExecuteNonQuery();
        }
    }
}
