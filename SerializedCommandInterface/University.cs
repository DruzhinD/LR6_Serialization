using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SerializedCommandInterface
{
    [Serializable]
    [XmlRoot("University")]
    public class University
    {
        public University() { }

        [JsonInclude]
        [XmlElement("professorsList")]
        public List<Professor> professorsList { get; set; } = new();
    }
}
