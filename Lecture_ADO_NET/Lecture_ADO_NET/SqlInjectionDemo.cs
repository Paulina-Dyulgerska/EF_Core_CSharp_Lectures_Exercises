using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lecture_ADO_NET
{
    class SqlInjectionDemo
    {
        static void Main()
        {
            //Console.Write("Enter your username:");
            //string username = Console.ReadLine();
            //Console.Write("Enter your password:");
            //string password = Console.ReadLine();

            //string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=Service;Integrated Security=true";
            //using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            //{
            //    sqlConnection.Open();
            //    string command = "SELECT COUNT(*) FROM Users WHERE Username = '" + username
            //        + "' AND [Password] = '" + password + "';";
            //    SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            //    int usersCount = (int)sqlCommand.ExecuteScalar();
            //    //ExecuteScalar pone nqma da mu dade vyzmojnost na evil hackera da pravi Insert, Update i DELETE v/u DB-a mu,
            //    //zashtoto samo ExecuteNonQuery() pozwolqwa takiwa neshta. zatowa proverkite se prawq s ExecuteScalar() i zatowa
            //    //DBite se kradat i po-rqdko mogat da bydat iztriti!!!
            //    Console.WriteLine(usersCount);
            //    //pri realen vhod i zapazena v DB-a password, mi vyrna towa:
            //    //Enter your username: ealpine0
            //    //Enter your password:b8eYD1a7R44
            //    // 1 //t.e. znam che ima edin potrebitel s takowa ime i parola v tazi DB!!!

            //    if (usersCount > 0)
            //    {
            //        Console.WriteLine("Welcome to our secret data!");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Access forbidden!");
            //    }
            //}
            ////Eto taka si napravih sama SQL Injection kato vyvedoh tozi user i parola:
            ////Enter your username: ' OR 1=1 -- //s ' zatvorih stringa na username i posle slojih -- na wsichko, koeto se
            ////pishe natatyk v koda i taka go zakomentirah. v rezultat commandata si stana validna i se izpylni i mi se dade
            ////login v systemata!!!! Passworda ne se checkva, toj e samo komentar veche!!!
            ////Enter your password: sda
            ////20
            ////Welcome to our secret data!


            //Kak da preventna SQL Injection:
            Console.Write("Enter your username:");
            string username = Console.ReadLine();
            Console.Write("Enter your password:");
            string password = Console.ReadLine();

            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=Service;Integrated Security=true";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                //tova znachi - pravi tazi zaqwka, ama az polse shte ti dam parametrite j!!
                SqlCommand sqlCommand =
                    new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = '@username' AND Password = '@password'", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@username", username);
                sqlCommand.Parameters.AddWithValue("@password", password);

                int usersCount = (int)sqlCommand.ExecuteScalar();
                Console.WriteLine(usersCount);

                if (usersCount > 0)
                {
                    Console.WriteLine("Welcome to our secret data!");
                }
                else
                {
                    Console.WriteLine("Access forbidden!");
                }
            }
        }
    }
}
