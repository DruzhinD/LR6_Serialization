using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace SerializedCommandInterface
{
    [Serializable]
    [XmlRoot("Professor")]
    public class Professor
    {
        [JsonIgnore]
        //счетчик
        public static int counter;

        [JsonIgnore]
        //количество преподавателей
        public static int amount;

        [JsonIgnore]
        //уникальный номер преподавателя
        public int Id { get; }

        [XmlElement("lName")]
        //фамилия
        public string LastName { get; set; }

        [XmlAttribute("fName")]
        //имя преподавателя
        public string FirstName { get; set; }

        [XmlAttribute("sName")]
        //отчество
        public string SecondName { get; set; }

        [XmlAttribute("subject")]
        //дисциплина преподавателя
        public string Subject { get; set; }

        [XmlAttribute("DoE")]
        //дата трудоустройства
        public DateTime Employment { get; set; }
        
        [JsonIgnore]
        //количество месяцев, проведенных в университете
        public int PeriodEmployment
        {
            get
            { 
                int years = DateOnly.FromDateTime(DateTime.Now).Year - Employment.Year;
                int months = DateOnly.FromDateTime(DateTime.Now).Month - Employment.Month;
                return years*12 + months;
            }
        }
        //для сериализации, но работает совсем не так, как надо
        public Professor()
        {
            Id = counter;
            counter++;
        }

        //конструктор с вводом всех полей
        public Professor(string lastName, string firstName, string secondName, string subject, DateTime employment)
        {
            LastName = lastName;
            FirstName = firstName;
            SecondName = secondName;
            Subject = subject;
            Employment = employment;

            Id = counter;
            counter++;
            amount++;
        }

        //Конструктор с датой трудоустройства по умолчанию
        public Professor(string lastName, string firstName, string secondName, string subject)
        {
            LastName = lastName;
            FirstName = firstName;
            SecondName = secondName;
            Subject = subject;

            //значение по умолчанию, т.е. сегодняшний день
            Employment = DateTime.Now; 

            Id = counter;
            counter++;
            amount++;
        }

        public override string ToString()
        {
            return $"{Id,-3} {LastName,-15} {FirstName,-15} {SecondName,-15} {Subject,-25} {Employment,-15:d}";
        }
    }
}
