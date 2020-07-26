using Lecture_EF_Introduction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Lecture_EF_Introduction
{
    class Program
    {
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory
                                                                    .Create(builder => { builder.AddConsole(); });

        static void Main(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<SoftUniContext>();
            //chrez optionsBuildera az opredelqm kyde da se vyrje EF Core i kak. S nego moga da smenqm connctionite kym
            //kakwo sa:
            optionsBuilder
                .UseSqlServer("Server=.\\SQLEXPRESS;Database=SoftUni;Integrated Security=true;")
                .UseLoggerFactory(MyLoggerFactory);

            using (var db = new SoftUniContext())
            {
                db.Database.EnsureCreated();//tova proverqwa ima li iskanata DB i ako q nqma, q syzdawa.

                Console.WriteLine(db.Departments.Count(x => x.Employees.Count() > 10));
                //3 vryshta
                //generiral mi e tazi zaqwka:
                //SELECT COUNT(*)
                //FROM[Departments] AS[d]
                //WHERE(
                //    SELECT COUNT(*)
                //    FROM[Employees] AS[e]
                //    WHERE[d].[DepartmentID] = [e].[DepartmentID]) > 10

                Console.WriteLine(db.Departments.Count());
                //16 vryshta
                //generiral mi e tazi zaqwka:
                //SELECT COUNT(*)
                //FROM[Departments] AS[d]

                var employeesInDepartmnet2 = db.Employees.Select(x =>
                new
                {
                    DepartmentName = x.Department.Name,
                    EmployeeFirstName = x.FirstName,
                    Count = x.EmployeesProjects.Count(s => s.EmployeeId == x.EmployeeId) //vzimam v kolko projecta raboti dadeniqt Employee!!!
                }).OrderByDescending(x => x.Count).ToList();
                //v cqlata gorna zaqwka, wsichko predi ToList() e virtualno, i v momenta, v kojto vikna ToList() dannite
                //se materializirat i se wzimat ot DB-a!!! Vsichko predi ToList() ne e realno vzeto to DB-a!!!
                //ako izplyuq edin ToList() taka - to vsichko sled nego se sluchwa localno, no moga
                //da pravq promeni po DB-a!!!!
                //var employeesInDepartmnet2 = db.Employees.ToList()Select(x =>
                //        new
                //        {
                //            DepartmentName = x.Department.Name,
                //            EmployeeFirstName = x.FirstName,
                //            Count = x.EmployeesProjects.Count(s => s.EmployeeId == x.EmployeeId) 
                //        }).OrderByDescending(x => x.Count).ToList();
                //s gornata i vqrna edna edinstvena zaqwka moga da spestq mnogo trafik po mrejata i da si vzema kakwoto iskam.
                //celta mi e da dyrja max dylgo zaqwkata w IQuerable i nakraq da sloja ToList() za da si vzema dannite.
                //drugi methods, koito slagat kraj na IQuerable sa First(), FirstOrDefault().
                //ako prisvoq resultata bez ToList() na nqkoq promenliva, informationa NE se materializira.
                //information-a se materializira ot promenlivata pri pyrviqt foreach s neq! Edva togawa se pravi
                //vzimaneto na dannite ot DB-a!!! Towa e taka, zashtoto resultata v promenlivata shte e ot type
                //IQuerable vse oshte i NQMA da e realizirana fizicheski zaqwka!!! Dokato stoi IQuerable, tova shte e
                //ne realna danna, a prosto podgotvqna virtualna zaqwka!!!! Za da ojivee trqbwa da izpylnq queryto i
                //da vzema dannite i resultata veche da NE E IQuerable!!!
                //tazi zaqwka mi e naprawilo na gorniq cod:
                //SELECT[d].[Name] AS[DepartmentName], [e0].[FirstName] AS[EmployeeFirstName], (
                // SELECT COUNT(*)
                //    FROM[EmployeesProjects] AS[e]
                //    WHERE([e0].[EmployeeID] = [e].[EmployeeID]) AND([e].[EmployeeID] = [e0].[EmployeeID])) AS[Count]
                //FROM[Employees] AS[e0]
                //INNER JOIN[Departments] AS[d] ON[e0].[DepartmentID] = [d].[DepartmentID]
                //ORDER BY(
                //    SELECT COUNT(*)
                //    FROM [EmployeesProjects] AS [e1]
                //    WHERE ([e0].[EmployeeID] = [e1].[EmployeeID]) AND([e1].[EmployeeID] = [e0].[EmployeeID])) DESC
            }
            Console.WriteLine();
        }
    }
}
