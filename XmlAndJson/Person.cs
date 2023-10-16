using System.Xml.Serialization;

namespace XmlAndJson
{
    //[Serializable]
    public class Person
    {
        public Person(decimal initialSalary)
        {
            Salary = initialSalary;
        }
        public Person() { }

        [XmlAttribute("fname")]
        public string? FirstName { get; set; }
        [XmlAttribute("lname")]
        public string? LastName { get; set; }
        [XmlAttribute("DoB")]
        public DateTime DateOfBirth { get; set; }

        public HashSet<Person> Children { get; set; }
        [XmlAttribute("salary")]
        public decimal Salary { get; set; }
    }

    //[Serializable]
    public class Persons
    {
        [XmlElement("people")]
        public List<Person> peopleList { get; set; }

        public Persons() { }
    }
}