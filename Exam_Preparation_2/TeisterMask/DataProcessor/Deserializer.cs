namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Serialization;
    using System.Globalization;
    using System.Text;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Linq;
    using System.Xml.Serialization;
    using System.IO;
    using TeisterMask.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var result = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(List<ProjectInputDto>), new XmlRootAttribute("Projects"));

            var projectsDtos = new List<ProjectInputDto>();

            using (var stream = new StringReader(xmlString))
            {
                projectsDtos = (List<ProjectInputDto>)serializer.Deserialize(stream);
            }

            var projectsToAdd = new List<Project>();

            foreach (var projectDto in projectsDtos)
            {
                //is valid open and due date; is opendate < duedate - all those things are not checked!!!!
                HasValidDate(projectDto.OpenDate, out DateTime projectOpenDate);
                HasValidDate(projectDto.DueDate, out DateTime projectDueDate);

                if (!IsValid(projectDto))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var projectToAdd = new Project
                {
                    Name = projectDto.Name,
                    OpenDate = projectOpenDate,
                    DueDate = (projectDto.DueDate == null || projectDto.DueDate == string.Empty) 
                                    ? (DateTime?)null : projectDueDate,
                };

                foreach (var taskDto in projectDto.Tasks)
                {
                    //•	If there are any validation errors for the task entity(such as invalid name, 
                    //open or due date are missing, task open date is before project open 
                    //date or task due date is after project due date), do not import it(only the task itself, 
                    //not the whole project) and append an error message to the method output.
                    //NOTE: Dates will be in format dd/ MM / yyyy, do not forget to use CultureInfo.InvariantCulture
                    if (!IsValid(taskDto))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    HasValidDate(taskDto.OpenDate, out DateTime taskOpenDate);
                    HasValidDate(taskDto.DueDate, out DateTime taskDueDate);

                    if (taskOpenDate < projectToAdd.OpenDate
                        || taskOpenDate > taskDueDate) //da go iztriq ako ne stawa
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if ((taskDueDate > projectToAdd.DueDate && projectDto.DueDate != string.Empty))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var taskToAdd = new Task
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType,
                    };

                    projectToAdd.Tasks.Add(taskToAdd);
                    //context.Tasks.Add(taskToAdd);
                }

                projectsToAdd.Add(projectToAdd);

                result.AppendLine(String.Format(SuccessfullyImportedProject,
                    projectToAdd.Name,
                    projectToAdd.Tasks.Count));
            }

            context.Projects.AddRange(projectsToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
            };

            var employeesDto = JsonConvert.DeserializeObject<List<EmployeeInputDto>>(jsonString, jsonSettings);

            var employeesToAdd = new List<Employee>();

            foreach (var employee in employeesDto)
            {
                if (!IsValid(employee))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var employeeToAdd = new Employee
                {
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };

                foreach (var taskId in employee.Tasks.Distinct())
                {
                    var taskToAdd = context.Tasks.FirstOrDefault(t => t.Id == taskId);

                    if (taskToAdd == null)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask
                    {
                        Employee = employeeToAdd,
                        Task = taskToAdd,
                    };

                    employeeToAdd.EmployeesTasks.Add(employeeTask);
                    //context.EmployeesTasks.Add(employeeTask);
                }

                employeesToAdd.Add(employeeToAdd);

                result.AppendLine(String.Format(SuccessfullyImportedEmployee,
                    employeeToAdd.Username,
                    employeeToAdd.EmployeesTasks.Count));
            }

            context.Employees.AddRange(employeesToAdd);

            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static bool HasValidDate(string dateString, out DateTime parsedDateTime)
        {
            return DateTime.TryParseExact(dateString,
                      "dd/MM/yyyy",
                      CultureInfo.InvariantCulture,
                      DateTimeStyles.None,
                      out parsedDateTime);
        }

        //public static string ImportProjects(TeisterMaskContext context, string xmlString)
        //{
        //    var result = new StringBuilder();

        //    XmlSerializer serializer = new XmlSerializer(typeof(List<ProjectInputDto>), new XmlRootAttribute("Projects"));

        //    var projectsDtos = new List<ProjectInputDto>();

        //    using (var stream = new StringReader(xmlString))
        //    {
        //        projectsDtos = (List<ProjectInputDto>)serializer.Deserialize(stream);
        //    }

        //    var projectsToAdd = new List<Project>();

        //    foreach (var projectDto in projectsDtos)
        //    {

        //        var hasValidOpenDate = HasValidDate(projectDto.OpenDate, out DateTime projectOpenDate);
        //        var hasValidDueDate = HasValidDate(projectDto.OpenDate, out DateTime projectDueDate);

        //        if (!IsValid(projectDto) || !hasValidOpenDate || !hasValidDueDate)
        //        {
        //            result.AppendLine(ErrorMessage);
        //            continue;
        //        }

        //        if (projectOpenDate > projectDueDate)
        //        {
        //            result.AppendLine(ErrorMessage);
        //            continue;
        //        };

        //        var projectToAdd = new Project
        //        {
        //            Name = projectDto.Name,
        //            OpenDate = projectOpenDate,
        //            DueDate = projectDto.DueDate == null ? (DateTime?)null : projectDueDate,
        //        };

        //        foreach (var taskDto in projectDto.Tasks)
        //        {
        //            //•	If there are any validation errors for the task entity(such as invalid name, 
        //            //open or due date are missing, task open date is before project open 
        //            //date or task due date is after project due date), do not import it(only the task itself, 
        //            //not the whole project) and append an error message to the method output.
        //            //NOTE: Dates will be in format dd/ MM / yyyy, do not forget to use CultureInfo.InvariantCulture
        //            if (!IsValid(taskDto))
        //            {
        //                result.AppendLine(ErrorMessage);
        //                continue;
        //            }

        //            HasValidDate(taskDto.OpenDate, out DateTime taskOpenDate);
        //            HasValidDate(taskDto.DueDate, out DateTime taskDueDate);

        //            if (taskOpenDate < projectToAdd.OpenDate || taskDueDate > projectToAdd.DueDate
        //                || taskOpenDate > taskDueDate //da go iztriq ako ne stawa
        //                )
        //            {
        //                result.AppendLine(ErrorMessage);
        //                continue;
        //            }

        //            var taskToAdd = new Task
        //            {
        //                Name = taskDto.Name,
        //                OpenDate = taskOpenDate,
        //                DueDate = taskDueDate,
        //                ExecutionType = (ExecutionType)taskDto.ExecutionType,
        //                LabelType = (LabelType)taskDto.LabelType,
        //            };

        //            projectToAdd.Tasks.Add(taskToAdd);
        //        }

        //        projectsToAdd.Add(projectToAdd);

        //        result.AppendLine(String.Format(SuccessfullyImportedProject,
        //            projectToAdd.Name,
        //            projectToAdd.Tasks.Count));
        //    }

        //    context.Projects.AddRange(projectsToAdd);

        //    //context.SaveChanges();

        //    return result.ToString().TrimEnd();
        //}

    }
}