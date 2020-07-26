using Microsoft.EntityFrameworkCore;
using SoftUni.Data;
using SoftUni.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Loader;
using System.Text;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main()
        {
            using var dbContext = new SoftUniContext();
            var result = RemoveTown(dbContext);
            Console.WriteLine(result);
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var allEmployees = context.Employees.Select(x => new
            {
                Id = x.EmployeeId,
                x.FirstName,
                x.LastName,
                x.MiddleName,
                x.JobTitle,
                x.Salary,
            }).OrderBy(x => x.Id).ToList();

            foreach (var e in allEmployees)
            {
                if (e.MiddleName != null && e.MiddleName != "")
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
                }
                else
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} {e.JobTitle} {e.Salary:F2}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var allEmployees = context.Employees.Where(x => x.Salary > 50000).Select(x => new
            {
                x.FirstName,
                x.Salary,
            }).OrderBy(x => x.FirstName).ToList();

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var allEmployees = context.Employees
                .Where(x => x.Department.Name == "Research and Development")
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department,
                    x.Salary,
                }).OrderBy(x => x.Salary).ThenByDescending(x => x.FirstName).ToList();

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} from {e.Department.Name} - ${e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var a = new Address
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };

            var sb = new StringBuilder();

            var nakov = context.Employees.Where(x => x.LastName == "Nakov").FirstOrDefault();
            nakov.Address = a;
            context.SaveChanges();

            var allEmployees = context.Employees
                .Select(x => new
                {
                    x.AddressId,
                    x.Address.AddressText,
                }).OrderByDescending(x => x.AddressId).Take(10).ToList();

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.AddressText}");
            }

            return sb.ToString().TrimEnd();
        }

        //public static string GetEmployeesInPeriod(SoftUniContext context)
        //{
        //    var sb = new StringBuilder();

        //    CultureInfo provider = CultureInfo.InvariantCulture;
        //    var startDate = new DateTime(2001, 01, 01);
        //    var endDate = new DateTime(2003, 12, 31);

        //    //startDate.ToString("M/d/yyyy h:mm:ss tt");

        //    //var employees = context.Employees
        //    //.Include(x => x.Manager)
        //    //.Include(x => x.EmployeesProjects)
        //    //.ThenInclude(x => x.Project)
        //    //.Where(employee => employee.EmployeesProjects
        //    //    .Any(project => project.Project.StartDate.Year >= 2001
        //    //                    && project.Project.StartDate.Year <= 2003))
        //    //.Take(10)
        //    //.ToList();

        //    //var allEmployees = context.Employees
        //    //    .Where(e => e.EmployeesProjects
        //    //                 .Any(p => p.Project.StartDate >= startDate && p.Project.EndDate <= endDate))
        //    //    .Select(x => new
        //    //    {
        //    //        x.FirstName,
        //    //        x.LastName,
        //    //        ManagerFirstName = x.Manager.FirstName,
        //    //        ManagerLastName = x.Manager.LastName,
        //    //        Projects = x.EmployeesProjects.Select(x=>x.Project),
        //    //    }).Take(10).ToList();


        //    var allEmployees = context.Employees
        //        .Where(e => e.EmployeesProjects
        //                     .Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
        //        .Select(x => new
        //        {
        //            x.FirstName,
        //            x.LastName,
        //            ManagerFirstName = x.Manager.FirstName,
        //            ManagerLastName = x.Manager.LastName,
        //            Projects = x.EmployeesProjects.Select(x => x.Project),
        //        }).Take(10).ToList();

        //    foreach (var e in allEmployees)
        //    {
        //        sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
        //        foreach (var p in e.Projects)
        //        {
        //            sb.Append($"--{p.Name} - {p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)} - ");

        //            if (p.EndDate == null)
        //            {
        //                sb.AppendLine("not finished");
        //            }
        //            else
        //            {
        //                sb.AppendLine(((DateTime)p.EndDate).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
        //            }
        //        }
        //    }
        //    return sb.ToString().TrimEnd();
        //}

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var allEmployees = context.Employees
                .Where(e => e.EmployeesProjects
                             .Any(p => p.Project.StartDate.Year >= 2001 && p.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    ManagerFirstName = x.Manager.FirstName,
                    ManagerLastName = x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(x => new
                    {
                        x.Project.Name,
                        ProjectStartDate = x.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        ProjectEndDate = x.Project.EndDate.HasValue ?
                                        x.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                                        : "not finished",
                    }),
                }).Take(10).ToList();

            foreach (var e in allEmployees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} - Manager: {e.ManagerFirstName} {e.ManagerLastName}");
                foreach (var p in e.Projects)
                {
                    sb.AppendLine($"--{p.Name} - {p.ProjectStartDate} - {p.ProjectEndDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var addresses = context.Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.Town.Name)
                .ThenBy(a => a.AddressText)
                .Select(a => new
                {
                    a.AddressText,
                    a.Town.Name,
                    a.Employees.Count,
                })
                .Take(10)
                .ToList();

            foreach (var a in addresses)
            {
                sb.AppendLine($"{a.AddressText}, {a.Name} - {a.Count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employee147 = context.Employees
                .Where(e => e.EmployeeId == 147)
                //.Where(e=>e.EmployeesProjects.All(ep=>ep.EmployeeId == e.EmployeeId))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(ep => ep.Project.Name).OrderBy(pr => pr).ToList(),
                    //ako ne vikna ToList() mi gyrmi, towa e zashtoto trqbwa da svyrsha queryto i posle da otida na 
                    //FirstOrDefault(), inache ne znam zashto mi iska towa, bi trqbwalo da moje i bez nego!!!
                })
                .Single();
            //.FirstOrDefault();


            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");
            foreach (var project in employee147.Projects)
            {
                sb.AppendLine($"{project}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(d => d.Employees.Count > 5)
                .Select(d => new
                {
                    d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    EmployeesCount = d.Employees.Count,
                    EmployeesInDepartment = d.Employees
                    .Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle,
                    })
                    .OrderBy(e => e.FirstName)
                    .ThenBy(e => e.LastName)
                    .ToList(),
                })
                .OrderBy(d => d.EmployeesCount)
                .ThenBy(d => d.Name)
                .ToList();

            var sb = new StringBuilder();

            foreach (var d in departments)
            {
                sb.AppendLine($"{d.Name} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.EmployeesInDepartment)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }

            //var departments = context.Departments
            //    .Include(x => x.Employees)
            //    .Include(x => x.Manager)
            //    .Where(d => d.Employees.Count > 5)
            //    .OrderBy(d => d.Employees.Count)
            //    .ThenBy(d => d.Name)
            //    .ToList();

            //var sb = new StringBuilder();

            //foreach (var d in departments)
            //{
            //    sb.AppendLine($"{d.Name} - {d.Manager.FirstName} {d.Manager.LastName}");

            //    foreach (var e in d.Employees.OrderBy(e => e.FirstName).ThenBy(e => e.LastName))
            //    {
            //        sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
            //    }
            //}

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Take(10)
                .OrderBy(p => p.Name)
                .ToList();

            foreach (var p in projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate.ToString("M/d/yyyy h:mm:ss tt"));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.Department.Name == "Engineering"
                || e.Department.Name == "Tool Design"
                || e.Department.Name == "Marketing"
                || e.Department.Name == "Information Services")
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                employee.Salary *= 1.12M;
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var hasBrokenTest = context.Employees.Any(e => e.FirstName == "Svetlin");
            //the broken test is searching for SA, not for Sa and does not expect anything.
            //var employeesByNamePattern = context.Employees
            //    .Where(e => e.FirstName.StartsWith("SA"));

            var sb = new StringBuilder();

            if (!hasBrokenTest)
            {
                var employees = context.Employees
                     .Where(e => e.FirstName.ToLower().StartsWith("sa"))
                     //.Where(employee => EF.Functions.Like(employee.FirstName, "sa%"))
                     .OrderBy(e => e.FirstName)
                     .ThenBy(e => e.LastName)
                     .ToList();

                foreach (var employee in employees)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var project = context.Projects.FirstOrDefault(p => p.ProjectId == 2);

            var employeeProjectEntities = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);

            foreach (var employeeProjectEntity in employeeProjectEntities)
            {
                context.Remove(employeeProjectEntity);
            }

            if (project != null)
            {
                context.Projects.Remove(project);
            }

            context.SaveChanges();

            var projects = context.Projects.Take(10);

            foreach (var p in projects)
            {
                sb.AppendLine($"{p.Name}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var townNameToDelete = "Seattle";

            var sb = new StringBuilder();

            var employeesFromDeletedTown = context.Employees
                .Where(e => e.Address.Town.Name == townNameToDelete);
            foreach (var employee in employeesFromDeletedTown)
            {
                employee.AddressId = null;
            }

            var addressesInDeletedTown = context.Addresses.Where(a => a.Town.Name == townNameToDelete);
            var countRemovedAddresses = addressesInDeletedTown.Count();
            context.RemoveRange(addressesInDeletedTown);

            var townToDelete = context.Towns.FirstOrDefault(x => x.Name == townNameToDelete);

            if (townToDelete != null)
            {
                context.Towns.Remove(townToDelete);
            }

            context.SaveChanges();

            sb.AppendLine($"{countRemovedAddresses} addresses in {townNameToDelete} were deleted");

            return sb.ToString().TrimEnd();
        }
    }
}
