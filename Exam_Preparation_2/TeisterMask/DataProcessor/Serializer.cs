namespace TeisterMask.DataProcessor
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
            var projects = context.Projects
                .Where(p => p.Tasks.Count > 0)
                .Select(p => new ProjectExportDto
                {
                    ProjectName = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate != null ? "Yes" : "No",
                    Tasks = p.Tasks
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
            };

            var employees = context.Employees
                .Where(e => e.EmployeesTasks.Any(t => t.Task.OpenDate >= date))
                .Select(e => new
                {
                    e.Username,
                    Tasks = e.EmployeesTasks
                    .Where(et => et.Task.OpenDate >= date)
                    .OrderByDescending(et => et.Task.DueDate)
                    .ThenBy(et => et.Task.Name)
                    .Select(et => new
                    {
                        TaskName = et.Task.Name,
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        LabelType = Enum.GetName(typeof(LabelType), et.Task.LabelType),
                        ExecutionType = Enum.GetName(typeof(ExecutionType), et.Task.ExecutionType),
                    })
                    .ToList()
                })
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