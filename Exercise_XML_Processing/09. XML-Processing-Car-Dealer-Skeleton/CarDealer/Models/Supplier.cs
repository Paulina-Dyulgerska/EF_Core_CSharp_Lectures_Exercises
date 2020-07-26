namespace CarDealer.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("Supplier")]
    public class Supplier
    {
        public Supplier()
        {
            this.Parts = new HashSet<Part>();
        }

        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }

        [XmlIgnore]
        public virtual ICollection<Part> Parts { get; set; }

        ////tova e nachin da zaobikolq fakta, che XmlSerializera NE moje da serializira Interfaces kato ICollection!!!
        //[NotMapped]
        //[XmlArray("Parts")]
        //[XmlArrayItem("Part")]
        ////[Browsable(false), EditorBrowsable(EditorBrowsableState.Never), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //public List<Part> PartsArray
        //{
        //    get
        //    {
        //        if (Parts == null)
        //            return null;
        //        return Parts.ToList();
        //    }
        //    set
        //    {
        //        Parts = value;
        //    }
        //}
    }
}
