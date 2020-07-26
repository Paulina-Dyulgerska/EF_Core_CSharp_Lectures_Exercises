namespace Lecture_ADO_NET
{
    using Microsoft.Data.SqlClient;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=.\SQLEXPRESS;Initial Catalog=SoftUni;Integrated Security=true";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                //sqlConnection.Open();
                //string command = "SELECT COUNT(*) FROM Employees";
                //SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                //object result = sqlCommand.ExecuteScalar();
                //Console.WriteLine(result); //293
                ////cast na resulta, za da ne e object:
                //int result1 = (int)result;
                //Console.WriteLine(result1);
                //int? result2 = result as int?;
                ////Console.WriteLine(result2); //0

                ////ako imam select, kojto vryshta tablica, trqbwa da polzwam ExecuteReader():
                //sqlConnection.Open();
                //string command = "SELECT * FROM Employees WHERE FirstName LIKE 'N%'";
                //SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                ////object result = sqlCommand.ExecuteScalar(); //vryshta 1, ako imam rezultat v tablichen vid, zatowa polzwam ExecuteReader()
                ////Console.WriteLine(result);
                ////reader-a trqbwa da mi e v using, zashtoto toj zadyljitelno trqbwa da se zatwori pri priklyuchvane na rabotata
                ////mu, inache nqma da mi dade da pravq 2-ri read pri nezatworen pyrwi!!!!
                //using (SqlDataReader reader = sqlCommand.ExecuteReader())
                ////v reader mi se namira tekushtiqt red ot tablicata!!! Vseki
                ////red ot tablicata, kojto e zapisan v reader, e ot type object!!! Pak trqbwa da convertna reda!!!
                //{
                //    while (reader.Read()) //dokato Read() moje da chete - Read() vryshta bool
                //    {
                //        string firstName = (string)reader[1];
                //        //po struktura na tablicata Employees, 0-levata kolona e Id-to, a 1-vata
                //        //kolona e FirstName, zatowa iskam [1].
                //        Console.WriteLine(firstName); //Nitin
                //                                      //Nancy
                //                                      //Nuan
                //                                      //Nicole
                //    }
                //}


                ////moga az da si pravq tablica v select i da rabotq s nejnite koloni s indexaciq ot 0 natatyk:
                //sqlConnection.Open();
                //string command = "SELECT [FirstName], [LastName] FROM Employees WHERE FirstName LIKE 'N%'";
                //SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                //using (SqlDataReader reader = sqlCommand.ExecuteReader())
                ////v reader mi se namira tekushtiqt red ot tablicata!!! Vseki
                //{
                //    while (reader.Read()) //dokato Read() moje da chete - Read() vryshta bool
                //    {
                //        //string firstName = (string)reader[0]; //veche 0-levata kolona mi e FirstName
                //        //string lastName = (string)reader[1]; //veche 1-vata kolona mi e LastName
                //        //Console.WriteLine(firstName + ' ' + lastName);
                //        ////Nitin Mirchandani
                //        ////Nancy Anderson
                //        ////Nuan Yu
                //        ////Nicole Holliday

                //        //moga az da si vzimam kolonite i po imena, zashtoto reader gi dyrji kato rechni i imenata na kolonite
                //        //sa kato Key v Dictionary. Predimstvoto na towa e, che bez znachenie kakyv index ima kolonata, toj shte
                //        //mi vyrne pravilnata, kogato q tyrsq po imeto j!!!
                ////reader e object, kojto ima propertyta, koito sa ravni na imenata na kolonite v prochetenata tablica!!!
                ////az si vikam tezi propertita na reader sys syntaxisa reader["imetoNaKolonataProperty"] i se grija da q cast-na
                ////podhodqshto!!!!
                //        string firstName = (string)reader["FirstName"];
                //        string lastName = (string)reader["LastName"];
                //        Console.WriteLine(firstName + ' ' + lastName);
                //        //Nitin Mirchandani
                //        //Nancy Anderson
                //        //Nuan Yu
                //        //Nicole Holliday
                //    }
                //}


                //ExecuteNonQuery(): Ako usera mi nqma permitions da pravi promeni po DB-a, to shte me izhwyrli, no az sega si 
                //naprawih connectiona s Integrated Security=true i zatowa nqma da me izhwyrli!!!
                sqlConnection.Open();
                string command = "SELECT [FirstName], [LastName], Salary FROM Employees WHERE FirstName LIKE 'N%'";
                SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal salary = (decimal)reader["Salary"];
                        Console.WriteLine(firstName + ' ' + lastName + " - " + salary);
                        //Nitin Mirchandani -14000.0000
                        //Nancy Anderson -12500.0000
                        //Nuan Yu -11000.0000
                        //Nicole Holliday -15000.0000
                    }
                }

                SqlCommand updateSalary = new SqlCommand("UPDATE Employees SET Salary = Salary + 55", sqlConnection);
                int numberUpdatedRows = updateSalary.ExecuteNonQuery(); //taka executvam commandata update salary!!!
                Console.WriteLine($"Salary updated for {numberUpdatedRows} employees");
                //Salary updated for 293 employees

                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];
                        decimal salary = (decimal)reader["Salary"];
                        Console.WriteLine(firstName + ' ' + lastName + " - " + salary);
                        //Nitin Mirchandani -14055.0000
                        //Nancy Anderson -12555.0000
                        //Nuan Yu -11055.0000
                        //Nicole Holliday -15055.0000
                    }
                }
            }
        }
    }
}
