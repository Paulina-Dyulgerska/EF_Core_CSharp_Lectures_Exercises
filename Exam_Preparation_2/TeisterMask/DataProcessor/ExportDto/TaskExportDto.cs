using System.Xml.Serialization;

namespace TeisterMask.DataProcessor.ExportDto
{
    [XmlType("Task")]
    public class TaskExportDto
    {
        public string Name { get; set; }

        public string Label { get; set; }
    }
}