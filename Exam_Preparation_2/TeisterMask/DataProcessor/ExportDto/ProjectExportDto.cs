using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Project")]
   public class ProjectExportDto
    {
        public string ProjectName { get; set; }

        [XmlAttribute]
        public int TasksCount { get; set; }

        public string HasEndDate { get; set; }

        public TaskExportDto[] Tasks { get; set; }

    }
}
