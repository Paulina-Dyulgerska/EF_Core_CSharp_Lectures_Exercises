﻿namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Microsoft.SqlServer.Server;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using TeisterMask.Data.Models.Enums;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            ////taka minava v Judge, no ne minawa pri men, da iztriq ToList()-ite, za da mi raboti:
            var projects = context.Projects
                .ToArray() //tova go pisha samo za da mine v DB-a, kojto se polzwa v Judge
                .Where(p => p.Tasks.Count > 0)
                .Select(p => new ProjectExportDto
                {
                    ProjectName = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
                    //.ToArray() //tova go pisha samo za da mine v DB-a, kojto se polzwa v Judge
                    .Select(t => new TaskExportDto
                    {
                        Name = t.Name,
                        Label = Enum.GetName(typeof(LabelType), t.LabelType),
                    })
                    .OrderBy(t => t.Name)
                    .ToArray(),
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            var xml = new StringBuilder();

            var serializer = new XmlSerializer(typeof(ProjectExportDto[]), new XmlRootAttribute("Projects"));
            var namespases = new XmlSerializerNamespaces();
            namespases.Add(string.Empty, string.Empty);

            using (var writer = new StringWriterUtf8(xml))
            {
                serializer.Serialize(writer, projects, namespases);
            }
            return xml.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new DefaultNamingStrategy()
                },
                NullValueHandling = NullValueHandling.Ignore,
                Culture = CultureInfo.InvariantCulture,
                Formatting = Formatting.Indented,
                //DateFormatString = "dd-MM-yyyy",
            };

            ////taka minava v Judge, no ne minawa pri men:
            //var employees = context.Employees
            //    .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
            //    //.ToList() //tova go pisha samo za da mine v DB-a, kojto se polzwa v Judge
            //    .Select(e => new
            //    {
            //        e.Username,
            //        Tasks = e.EmployeesTasks
            //        .Where(et => et.Task.OpenDate >= date)
            //        //.ToList() //tova go pisha samo za da mine v DB-a, kojto se polzwa v Judge
            //        .OrderByDescending(et => et.Task.DueDate)
            //        .ThenBy(et => et.Task.Name)
            //        .Select(et => new
            //        {
            //            TaskName = et.Task.Name,
            //            OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
            //            DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
            //            LabelType = Enum.GetName(typeof(LabelType), et.Task.LabelType),
            //            ExecutionType = Enum.GetName(typeof(ExecutionType), et.Task.ExecutionType),
            //        })
            //        .ToList()
            //    })
            //    .OrderByDescending(e => e.Tasks.Count)
            //    .ThenBy(e => e.Username)
            //    .Take(10)
            //    .ToList();


            //Taka moga da si izmykvam dannite za wseki edin Task ot mejdinnata tablica EmployeeTask i 
            //posle rabotq ne s EmployeeTask, a s chist Task!!!!
            //Tova moga da go pravq za vseki takyv sluchaj, v kojto mi trqbwat samo ednite obekti, v
            //sluchaq mi trqbwa samo Task, zashtoto Employee veche
            //e qsno koe e, poneje se namiram v tozi Employee weche i vzimam samo negovite Tasks tuk!!!!
            ////taka minava i v Judge, i pri men:
            var employees = context.Employees
                  .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                  .Select(e => new
                  {
                      e.Username,
                      Tasks = e.EmployeesTasks
                      .Where(et => et.Task.OpenDate >= date)
                      .Select(et => et.Task) //izmykvam dannite za wseki edin Task ot mejdinnata tablica EmployeeTask!!!!
                                             //zaradi towa minawa v Judge, bez da trqbwa da pisha ToList() tuk!!!
                      .OrderByDescending(t => t.DueDate)
                      .ThenBy(t => t.Name)
                      .Select(t => new
                      {
                          TaskName = t.Name,
                          OpenDate = t.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                          DueDate = t.DueDate.ToString("d", CultureInfo.InvariantCulture),
                          LabelType = Enum.GetName(typeof(LabelType), t.LabelType),
                          //moje i taka da se vzeme stojnostta na enum-a:
                          //LabelType = t.LabelType.ToString(),
                          ExecutionType = Enum.GetName(typeof(ExecutionType), t.ExecutionType),
                          //ExecutionType = t.ExecutionType.ToString(),
                      })
                      .ToList()
                  })
                  .ToList()
                  .OrderByDescending(e => e.Tasks.Count)
                  .ThenBy(e => e.Username)
                  .Take(10)
                  .ToList();

            var json = JsonConvert.SerializeObject(employees, jsonSettings);

            return json.ToString().TrimEnd();
        }
    }

    // Subclass the StringWriter class and override the default encoding. 
    // This allows us to produce XML encoded as UTF-8.
    public class StringWriterUtf8 : System.IO.StringWriter
    {
        public StringWriterUtf8(StringBuilder xml) : base(xml)
        {
        }
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}