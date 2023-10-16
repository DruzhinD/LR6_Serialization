using System;
using System.Xml.Serialization;
using static System.Console;
using NewJson = System.Text.Json.JsonSerializer;

namespace XmlAndJson
{
    internal class Program
    {
        static Persons people = new()
        {
            peopleList = new()
            {
            new(30000M)
            {
                FirstName = "Alice",
                LastName = "Domik",
                DateOfBirth = new(1986, 12, 12),
            },
            new(25000M)
            {
                FirstName = "Dima",
                LastName = "Danilov",
                DateOfBirth = new(2000, 2, 22),
            },
            new(34213M)
            {
                FirstName = "Andrey",
                LastName = "Evgenov",
                DateOfBirth = new(1996, 5, 19),
                Children = new()
                {
                    new(0M)
                    {
                        FirstName = "Мила",
                        LastName = "Evgenova",
                        DateOfBirth = new(2015, 04, 05),

                    }
                }
            },
        }

        };

        static void Main(string[] args)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\");
            path = Path.GetFullPath(path);

            XmlSerialization(Path.Combine(path, "people.xml"));
            XmlDeserialization(Path.Combine(path, "people.xml"));

            //JsonSerialization(Path.Combine(path, "people.json"));
            //NewJsonDeserialization(Path.Combine(path, "people.json"));
        }

        static void XmlSerialization(string path)
        {
            XmlSerializer xs = new(people.GetType());
            using (FileStream stream = File.Create(path))
            {
                xs.Serialize(stream, people);
            }

            WriteLine("Written {0:N0} bytes of XML to {1}",
                arg0: new FileInfo(path).Length,
                arg1: path);
            WriteLine();
            WriteLine(File.ReadAllText(path));
        }

        static void XmlDeserialization(string path)
        {
            XmlSerializer xs = new(people.GetType());

            using (FileStream xmlLoad = File.Open(path, FileMode.Open))
            {
                Persons? loadedPeople = xs.Deserialize(xmlLoad) as Persons;

                if (loadedPeople != null)
                {
                    foreach (Person p in loadedPeople.peopleList)
                        WriteLine("{0} has {1} children.",
                            p.LastName, p.Children?.Count ?? 0);
                    //foreach (Person p in loadedPeople.peopleList)
                    //    WriteLine($"{p.LastName}, {p.FirstName}, {p.DateOfBirth}, {p.Children.ToString()}");
                }
            }
        }

        static void JsonSerialization(string path)
        {
            using (StreamWriter jsonStream = File.CreateText(path))
            {
                //создаем объект, который будет форматироваться как json
                Newtonsoft.Json.JsonSerializer jss = new();

                //сериализуем объектный граф в строку
                jss.Serialize(jsonStream, people);
            }

            WriteLine();
            WriteLine("Written {0:N0} bytes of JSON to: {1}",
                arg0: new FileInfo(path).Length,
                arg1: path);

            WriteLine(File.ReadAllText(path));
        }

        static async void NewJsonDeserialization(string path)
        {
            using (FileStream jsonLoad = File.Open(path, FileMode.Open))
            {
                //десериализуем объектный граф в список лиц
                List<Person>? loadedPeople = await NewJson.DeserializeAsync(
                    utf8Json: jsonLoad, returnType: typeof(List<Person>)) as List<Person>;
                WriteLine();
                if (loadedPeople is not null)
                {
                    foreach (Person p in loadedPeople)
                    {
                        WriteLine("{0} has {1} children.",
                            p.LastName, p.Children?.Count ?? 0);
                    }
                }
            }
        }
    }
}