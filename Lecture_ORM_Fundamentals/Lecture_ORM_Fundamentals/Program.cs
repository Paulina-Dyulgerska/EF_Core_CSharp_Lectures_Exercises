namespace Lecture_ORM_Fundamentals
{
    using Lecture_ORM_Fundamentals.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;

    class Program
    {
        static void Main(string[] args)
        {
            var dbContex = new StudentsDBContext();
            dbContex.Database.EnsureCreated(); //vij ima li takawa DB i ako nqma q syzdaj!
                                               //dbContex.Courses.Add(new Course { Name = "Entity Framework Core" });
                                               //dbContex.Courses.Add(new Course { Name = "SQL Server" });
                                               ////za da se zapishat kursovete mi, az trqbwa specialno da kaja towa:
                                               //dbContex.SaveChanges(); //!!! ako ne napisha towa, nqma da mi se insertne nishto v DB-a!!!
                                               ////SaveChanges() vinagi raboti s transaction!!! Promenite se zapisvat prez transaction vinagi!!!

            ////kak se pravi UPDATE:
            //Course course = dbContex.Courses.FirstOrDefault(x => x.Id == 1);
            //course.Name = "Polq changed the Name!!!";
            //dbContex.SaveChanges();
            ////veche 1-ivqt mi kurs e s moeto ime!

            ////kak se pravi DELETE:
            //course = dbContex.Courses.FirstOrDefault(x => x.Id == 1);
            //dbContex.Courses.Remove(course);
            //dbContex.SaveChanges();
            ////veche 1-ivqt kurs e iztrit!

            //ako nqmam neshto kato functionalnost v EFCore, az moga da si izpylnq i SQL cod. Naprimer, ako mi
            //lipswa nqkoq function, moga da si q napisha taka:
            //dbContex.Database.ExecuteSqlRaw("DELETE FROM Courses");
            //dbContex.Database.ExecuteSqlRaw("INSERT INTO Courses VALUES ('C# Advanced');");


            //EFCore ima t.nar graf analyser, kojto gleda kakwo dobavqm i ako edno entity nosi v sebe si nov za DB entity, koeto
            //trqbwa da go ima v nqkoq druga tablica, to Graf Analyser-a shte zapishe i vtoroto novo entity v podhodqshtata za
            //nego tablica!!! Eto sega syzdawam ocenka i i davam neshtestvuvash student i kurs:
            dbContex.Grades.Add(new Grade
            {
                Student = new Student { FirstName = "Nikolay", LastName = "Kostov" }, //tozi student go nqmam, no graph analysera shte go  dobavi v DB-a
                Course = new Course { Name = "EF Core" }, //tozi course go nqmam, no graph analysera shte go  dobavi v DB-a
                GradeValue = 6.00M,
            });
            dbContex.SaveChanges(); //vkaraha se 3 novi zapisa v DB-a v syotvetnite im tablici!!!!

            //taka se pravi UPDATE:
            var gradeNiki = dbContex.Grades.FirstOrDefault(x => x.Student.FirstName == "Nikolay");
            gradeNiki.GradeValue = 5.59M;
            dbContex.SaveChanges();

            ////taka se pravi DELETE:
            //var gradeNikiForDelete = dbContex.Grades.FirstOrDefault(x => x.Student.FirstName == "Nikolay");
            //dbContex.Grades.Remove(gradeNikiForDelete);
            //dbContex.SaveChanges();

            ////DELETE moje da se pravi i bez da vzimam danni ot DB-a, v sluchaite, v koito tochno znam kakwo iskam da iztriq:
            //var courseToDelete = new Course { Id = 2 }; //syzadawam takyw object, kojto ima Id 2. 
            //dbContex.Courses.Remove(courseToDelete); //iztrih 2 course bez da go vzimam ot DB-a!!!Taka pestq trafic do DB-a!!!
            //dbContex.SaveChanges();

            //ako iskam da iztriq mnogo zapisi navednyj, pravq go s ExecuteSqlRow("DELETE FROM Courses");

            //vzimam si zapisi ot DB-a i si gi grupiram i si gi prawq kakwoto si iskam:
            var coursesGroups = dbContex.Courses
                .GroupBy(x => x.Name)
                .Select(x => new
                {
                    courseName = x.Key,
                    numberOfCourses = x.Count(),
                })
                .OrderByDescending(x => x.numberOfCourses);
            //tuk DbSet<Courses> shte naprawi takawa zaqwka kym DB-a:
            //SELECT Name, COUNT(*)
            //  FROM Courses
            //  GROUP BY Name
            //DbSet - a e mnogo umen, ako iskam ne vsichki hora, a samo nqkoj, toj nqma da vzeme wsichki hora i da prawi 
            //posle obrabotka za tezi,koito
            //mi trqbwat, a shte naprawi takyv SELECT, che da vzeme tochno towa, koeto mi trqbwa i nishto poveche!!!

            foreach (var course in coursesGroups) //foreach-a pravi za men ToList(); zashtoto az ne sym go napravila gore :)
                                                  //Ako si naprawq dannite na ToList(), sled nego weche kakwoto i da napisha, 
                                                  //to se izpylnqwa v/ u nowata collection List<>, a ne e
                                                  //chast ot zaqwkata mi i ne se prashata na sql servera mi!!!!
            {
                Console.WriteLine($"Course name: {course.courseName} => number of courses with that name: {course.numberOfCourses}");
            }

            //moga da si pravq slojen Grouping: da ne zabrawqm, che groupby raboti vyrhu kolectiona, kojto mu e podaden,
            //t.e.ako sloja select predi groupby, to groupby shte raboti vyrhu rezultata ot selecta. selecta e proekciq nova na
            //collectiona, vyrhu kojto prilagam tozi select i select vryshta nova collection.
            //osnovna razlika m/u LINQ i SQL e, che Selecta se pishe posleden w LINQ, dokato v SQL se pishe pyrvi. Ama towa e 
            //logichno, zashtoto v SQL nqmam kolection v/u koqto da rabotq, dokato w LINQ az imam veche gotowa collection, vyrhu
            //koqto prilagam kakwoto mi hrumne, v towa chislo i Select(). t.e. po syshtestwo, za da imam operaziq v/u danni, trqwba
            //da imam danni :)
            var gradesGroups = dbContex.Grades
                                .GroupBy(x => x.Student.FirstName + " " + x.Student.LastName)
                                .Where(x => x.Key.Contains("Polq")) //moje da si pisha where za filter na groupite.
                                .Select(x => new
                                {
                                    studentName = x.Key,
                                    sumGrades = x.Sum(g => g.GradeValue),
                                    countGrades = x.Count()
                                })
                                .OrderByDescending(x => x.sumGrades / x.countGrades);
            //tazi zaqwka se e generira la gornotto query:
            //SELECT([s].[FirstName] + N' ') + [s].[LastName] AS[studentName], 
            //SUM([g].[GradeValue]) AS[sumGrades], COUNT(*) AS[countGrades]
            //FROM[Grades] AS[g]
            //LEFT JOIN[Students] AS[s] ON[g].[StudentId] = [s].[Id]
            //GROUP BY([s].[FirstName] +N' ') + [s].[LastName]
            //HAVING CHARINDEX(N'Polq', ([s].[FirstName] + N' ') + [s].[LastName]) > 0
            //ORDER BY SUM([g].[GradeValue]) / CAST(COUNT(*) AS decimal(18, 2)) DESC

            foreach (var grade in gradesGroups) //foreach-a pravi za men ToList(); zashtoto az ne sym go napravila gore :)
            {
                Console.WriteLine($"Student name: {grade.studentName} => sum of grades: " +
                    $"{grade.sumGrades} => count grades: {grade.countGrades}");
            }

            //example za ICollection kato property na edin POCO:
            //dbContex.Students.Where(x => x.Grades.All(x => x.GradeValue > 4.50M));
            ////moga da rovq v property Grades na Student
            ////i da si pravq selection v nego, no towa ne e ot DB-a, a ot obekta v pametta - konkretnata instanciq na Student, za
            ////koqto pitam.
            //var studentsGrades = dbContex.Students.Where(x => x.Grades.Average(x => x.GradeValue) > 4.50M);
            //foreach (var student in studentsGrades) //foreach-a pravi za men ToList(); zashtoto az ne sym go napravila gore :)
            //{
            //    Console.WriteLine($"Student name: {student.FirstName} => grades:{String.Join(", ", student.Grades)}");
            //}

            bool exists = dbContex.Students.Any(x => x.FirstName.StartsWith("P"));
            Console.WriteLine(exists);

            int count = dbContex.Students.Count(x => x.FirstName == "Polq");
            Console.WriteLine(count); //1
            count = dbContex.Students.Count(x => x.FirstName == "pOlq");
            Console.WriteLine(count); //1, zashtoto SQL ne pravi razlika m/u malki i golemi bukvi!!!

            //ako promenq neshto v DB-a, i posle pravq select v/u dannite, no ne sym naprawila SaveChanges(), to v selecta
            //shte mi se vyrnat dannite taka, kakto biha bili, ako veche bqha promeneni v DB-a sys SaveChanges()!!! Towa e taka, 
            //zashtoto imam changeTracker i toj znae koe neshto kak se e promenilo zaradi moq cod i shte mi go vyrne
            //v promeneiq state. Vypreki towa, ako ne sloja nakraq SaveChanges(), to nqma da se smeni v DB-a. T.e. shte se smeni
            //samo v ramkite na programata mi i dokato e jiva syotvetnata promenliva!!!! Example:
            var firstStudent = dbContex.Students.FirstOrDefault();
            firstStudent.FirstName = "Test";
            var secondStudent = dbContex.Students.FirstOrDefault(x => x.FirstName == "Pesho");
            Console.WriteLine(firstStudent.FirstName); //Test
            //v pammeta na kompa e Test, no ako ne dam posle SaveChanges, imeto nqma da e Test v DB-a!!!
            //t.e. ako veche sym drypnala zapisa i sym go promenila, posle za tozi zapis mi se dawat danni ot pametta na programata
            //mi lokalno, syobrazno dannite v ChangeTracker-a, a ne se rovi pak v DB-a za tozi zapis. Towa pesti trafik!!!
            //towa podobrqwa performance. Nelepoto v primera e, che az mu kazwam da brykne w DB-a i da mi dade imeto na tozi, kojto
            //se kazwa "Pesho" - toj byrka i vijda, che ima promqna po Pesho i mi vryshta, che se kazwa "Test"!!!! Nelepo e. Ama to e
            //nelepo i towa da tyrsq tozi, deto go krystih Test, da se kazwa Pesho v DB-a. Tq zaqwkata mi e nelepa :) No e interesno.
            //ako iskam da vzema nanovo 1-viq student, no ot DB-a i da izlyja ORM-a i da ne rabotq s towa, deto toj si pazi,
            //trqbwa prosto da naprawq nowa zaqwka w nov DBContext!!!! taka:
            var student2 = new StudentsDBContext().Students.FirstOrDefault();
            Console.WriteLine(student2.FirstName); //Pesho - vrystha mi Peshe ot DB-a, a ne ot zapisa v secondStudent v pametta na
            //programata mi!!!!

        }
    }
}
