using System.Xml.Serialization;
using static System.Console;

namespace SerializedCommandInterface
{
    internal class StartProgram
    {
        static List<Professor> professors = new()
        {
            new("Иванов", "Иван", "Иванович", "Физика", new(year: 1999, month: 2, day: 9)),
            new("Александрова", "Александра", "Александровна", "Социология", new(year: 2011, month: 10, day: 16)),
            new("Петров", "Пётр", "Петрович", "Высшая математика", new(year: 2005, month: 4, day: 12))
        };

        static Dictionary<string, string> aboutCommands = new()
        {
            { "help", "получить справку по командам" },
            { "addprof",
                "Добавить нового преподавателя. \n (аргументы: Ф_И_О_дисциплина " +
                "ИЛИ Ф_И_О_дисциплина_дата трудоустройства)" },

            { "exit", "Завершить работу программы" },
            { "list", "Вывести список преподавателей. \n (аргументы: id ИЛИ id_'period')" },
            { "del",  "удалить информацию о преподавателе (аргументы: id)"}
        };

        static void Main(string[] args)
        {
            //Menu();

            //нужно разобраться с тем, что снизу
            Professors profs = new();
            profs.professorsList.Add(new Professor("Иванов", "Иван", "Иванович", "Физика", new DateOnly(2004, 2, 1)));
            profs.professorsList.Add(new Professor("Александрова", "Александра", "Александровна", "Социология", new DateOnly(2004, 2, 1)));
            profs.professorsList.Add(new Professor("Петров", "Пётр", "Петрович", "Высшая математика", new DateOnly(2004, 2, 1)));
            SerializeXML("file1.xml", profs);
            profs.professorsList.Clear();
            profs = DeserializeXML("file1.xml");
            Console.WriteLine($"{profs.professorsList[0].ToString()}");
        }

        static void Menu()
        {
            string[] command;
            do
            {
                Write(">");
                command = ReadLine().Split('|', '_');
                command[0] = command[0].ToLower(); //форматирование ввода в нижнем регистре
                switch (command[0])
                {
                    case "help":
                        Help(command);
                        break;
                    case "addprof":
                        AddProf(command);
                        break;
                    case "del":
                        Del(command);
                        break;
                    case "list":
                        List(command);
                        break;
                    case "exit":
                        WriteLine("Выход из программы...");
                        break;
                    default:
                        WriteLine("Неизвестная команда. Повторите ввод.");
                        WriteLine("help - для вызова справки.");
                        break;
                }
            }
            while (command[0] != "exit");
        }

        //Команда добавления нового преподавателя в список
        //addProf_Ф_И_О_Дисциплина_дата / addProf_Ф_И_О_Дисциплина
        static void AddProf(string[] command)
        {
            
            //проверяет соответствие количества аргументов
            if (command.Length < 5)
            {
                WriteLine($"Ошибка ввода: недостаточно аргументов " +
                    $"({command.Length} вместо необходимых 5). \n");
                return;
            }
            else if (command.Length == 5)
                professors.Add(new(command[1], command[2], command[3], command[4]));
            else if (command.Length == 6)
            {
                //проверка на соответствие введенного формата даты
                DateOnly date;
                if (DateOnly.TryParse(command[5], out date))
                {
                    professors.Add(new(command[1], command[2], command[3], command[4], date));
                }
                else
                {
                    WriteLine("Ошибка ввода: Неверный формат даты. \n");
                    return;
                }
            }
            else if (command.Length > 6)
            {
                WriteLine($"Ошибка ввода: аргументов больше, чем необходимо " +
                    $"({command.Length} вместе максимальных 5). \n");
                return;
            }
            WriteLine("Информация о преподавателе успешно добавлена. \n");
        }

        //Вывод доступных команд
        static void Help(string[] command)
        {
            WriteLine("Доступные команды:");
            foreach (KeyValuePair<string, string> com in aboutCommands)
            {
                WriteLine($"\t{com.Key}: {com.Value}");
            }
            WriteLine("Примечание: " +
                "\n 1)команды с аргументами вводятся через _ или |" +
                "\n 2)аргументы в апострофах ('') вводятся именно таким образом.");
            WriteLine();
        }

        //вывод списка преподавателей
        static void List(string[] command)
        {
            int id; //id преподавателя
            if (command.Length == 1)
            {
                WriteLine($"{"id",-3}|{"Фамилия",-15}|{"Имя",-15}|{"Отчество",-15}" +
                    $"|{"Дисциплина",-25}|{"Дата трудоустройства",-8}");
                WriteLine(new string('-', 90));
                foreach (Professor professor in professors)
                {
                    WriteLine(professor);
                }
                WriteLine();
            }
            else if (command.Length == 2 && int.TryParse(command[1], out id))
            {
                WriteLine($"Профессор: {FindById(id)}");
            }
            else if (command.Length == 3 && command[2] == "period" && int.TryParse(command[1], out id))
            {
                Professor professor = FindById(id);
                if (professor != null)
                {
                    WriteLine($"Профессор {professor.LastName} {professor.FirstName} {professor.SecondName}" +
                        $" работает в университете уже {professor.PeriodEmployment} месяц(-а)(-ев)");
                }
            }
            else
                WriteLine($"Неверно задан(-ы) аргумент(-ы).");
        }

        //удаление преподавателя по id
        static void Del(string[] command)
        {
            int id;
            if (command.Length < 2)
                WriteLine("Недостаточно аргументов");
            else if (int.TryParse(command[1], out id))
            {
                Professor prof = FindById(id);
                professors.Remove(prof);
                if (prof != null)
                    WriteLine($"Профессор с id = {id} успешно удален.");
                else
                    WriteLine($"Преподаватель с id={id} не найден.");
            }
            else
                WriteLine("Аргумент должен быть числом.");
        }

        //поиск преподавателя по ID
        static Professor FindById(int id)
        {
            foreach (Professor professor in professors)
                if (professor.Id == id)
                    return professor;
            return null;
            
        }

        static void SerializeXML(string fileName, Professors professors)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Professors));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                xml.Serialize(fs, professors);
            }
        }
        static Professors DeserializeXML(string fileName)
        {
            XmlSerializer xml = new XmlSerializer(typeof(Professors));
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                return (Professors)xml.Deserialize(fs);
            }
        }
    }
}