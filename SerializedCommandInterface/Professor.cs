using System.Xml.Serialization;

namespace SerializedCommandInterface
{
    [Serializable]
    public class Professor
    {
        //счетчик
        public static int counter;

        //количество преподавателей
        public static int amount;

        [XmlElement("Professor")]
        //уникальный номер преподавателя
        public int Id { get; }

        //фамилия
        public string LastName { get; set; }
        
        //имя преподавателя
        public string FirstName { get; set; }

        //отчество
        public string SecondName { get; set; }

        //дисциплина преподавателя
        string subject;
        public string Subject { get => subject; set => subject = value; }

        //дата трудоустройства
        public DateOnly Employment { get; set; }
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

        public Professor() { }

        //конструктор с вводом всех полей
        public Professor(string lastName, string firstName, string secondName, string subject, DateOnly employment)
        {
            LastName = lastName;
            FirstName = firstName;
            SecondName = secondName;
            this.subject = subject;
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
            this.subject = subject;

            //значение по умолчанию, т.е. сегодняшний день
            Employment = DateOnly.FromDateTime(DateTime.Now); 

            Id = counter;
            counter++;
            amount++;
        }

        public override string ToString()
        {
            return $"{Id, -3} {LastName,-15} {FirstName,-15} {SecondName,-15} {Subject,-25} {Employment,-15}";
        }

    }
}
