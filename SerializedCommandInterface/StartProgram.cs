using System.Text.RegularExpressions;
using System.Xml.Serialization;
using static System.Console;

namespace SerializedCommandInterface
{
    internal class StartProgram
    {
        //путь к директории файлов для сериализации
        static string path = Path.GetFullPath(Directory.GetCurrentDirectory() + @"\..\..\..\");

        static List<Professor> professors = new();

        static Dictionary<string, string> aboutCommands = new()
        {
            { "help", "получить справку по командам" },
            { "addprof",
                "Добавить нового преподавателя. \n (аргументы: Ф_И_О_дисциплина " +
                "ИЛИ Ф_И_О_дисциплина_дата трудоустройства)" },

            { "exit", "Завершить работу программы" },
            { "list", "Вывести список преподавателей. \n (аргументы: id ИЛИ id_'period')" },
            { "del",  "удалить информацию о преподавателе (аргументы: id)"},
            { "ser", "Сериализовать определенное количество преподавателей \n" +
                "с удалением их из памяти (списка в программе) с использованием xml или json \n" +
                 "для xml: ser_xml, для json: ser_json \n"},
            {"deser", "десериализовать информацию о преподавателях" }
        };

        static string[] aboutSerArgs = 
        {
            "аргументы: 1) индекс преподавателя_количество преподавателей",
            "2) индекс преподавателя_количество преподавателей_'save'",
            "для сохранения сериализованной части списка в списке преподавателей"
        };

        static string[] aboutDeserArgs =
        {
            "аргументы: 1) тип сериализатора",
            "2) тип сериализатора_'mod'",
            "mod необходим для десериализации файла с несуществующими/отсутствующими полями (3 задание ЛР№6)"
        };
        
        

        static void Main(string[] args)
        {
            InitializeProfessorsList();

            Menu();
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
                    case "ser":
                        Serializing(command);
                        break;
                    case "deser":
                        Deserialization(command);
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
                DateTime date;
                if (DateTime.TryParse(command[5], out date))
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
            //вывод всех команд
            if (command.Length == 1)
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
            //вывод более подробной информации об определенной команде
            else if (command.Length == 2 && aboutCommands.ContainsKey(command[1]))
            {
                WriteLine($"\t\"{command[1]}\": {aboutCommands[command[1]]}");
                if (command[1] == "ser")
                {
                    foreach (string arg in aboutSerArgs)
                        WriteLine($"\t {arg}");
                }
                else if (command[1] == "deser")
                {
                    foreach (string arg in aboutDeserArgs)
                        WriteLine($"\t {arg}");
                }
            }
            else
            {
                WriteLine("Неизвестная команда");
            }
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

        /// <summary>
        /// Метод для сериализации части списка преподавателей
        /// </summary>
        /// <param name="command">набор аргументов, см. help</param>
        static void Serializing(string[] command)
        {
            //индекс преподавателя
            int idProf;
            //количество преподавателей
            int amount;

            Regex indexesRegex = new(@"\d{1,3}");
            MatchCollection indexMatches = indexesRegex.Matches(string.Join('_', command));
            if (indexMatches.Count < 2)
            {
                WriteLine("Недостаточно аргументов. Передано {0} из 2 необходимых.",
                    indexMatches.Count);
                return;
            }
            else if (indexMatches.Count > 2)
            {
                WriteLine("Введено слишком много аргуметнов. Передано {0} из 2 необходимых.", 
                    indexMatches.Count);
                return;
            }
            else
            {
                //индекс преподавателя
                idProf = int.Parse(indexMatches[0].Value);
                //количество преподавателей
                amount = int.Parse(indexMatches[1].Value);
            }

            //проверка на существование преподавателя с таким id
            Professor isProfExist = FindById(idProf);
            if (isProfExist == null)
            {
                WriteLine($"Преподаватель с id={idProf} не найден.");
                return;
            }

            //проверка (относительно конечного индекса) на существование введенного количества преподавателей
            //находящихся после указанного, включая его самого
            //если НЕверно, то НЕсериализуем
            int profListIndex = professors.IndexOf(isProfExist);
            if (profListIndex + amount - 1 > professors.Count - 1) 
            {
                WriteLine($"Не удалось сериализовать {amount} преподавателей, следующих после {idProf}");
                return;

            }

            //выбор сериализатора
            if (command[1].ToLower() == "xml")
            {
                SerializeUniversity.SerializeXML(
                    Path.Combine(path, "university.xml"), professors.GetRange(profListIndex, amount));
            }
            else if (command[1].ToLower() == "json")
            {
                SerializeUniversity.SerializeJson(
                    Path.Combine(path, "university.json"), professors.GetRange(profListIndex, amount));
            }
            else
            {
                WriteLine($"Неверно указан сериализатор. {command[1]}, вместо xml/json.");
                return;
            }

            //проверка на наличие в команде слова save
            //при отсутствии - удаление сериализованных элементов
            if (command.Length == 5 && command[4].ToLower() == "save")
                return;
            else
                professors.RemoveRange(profListIndex, amount);
        }

        //внесенные изменения в файлы (добавленные поля/свойства) просто игнорируются
        /// <summary>
        /// метод десериализации xml/json
        /// </summary>
        /// <param name="command">набор аргументов, см. help</param>
        static void Deserialization(string[] command)
        {
            //временный путь
            string tempPath;

            List<Professor> newProfessors = new();
            if (command.Length > 3)
            {
                WriteLine("Неверно указан или отсутствует сериализатор.");
                return;
            }
            //десериализация xml
            else if (command[1].ToLower() == "xml")
            {
                if (command.Length == 3 && command[2] == "mod")
                    tempPath = Path.Combine(path, "universityModified.xml");
                else
                    tempPath = Path.Combine(path, "university.xml");

                newProfessors = SerializeUniversity.DeserializeXML(tempPath);
            }
            //десериализация json
            else if (command[1].ToLower() == "json")
            {
                if (command.Length == 3 && command[2] == "mod")
                    tempPath = Path.Combine(path, "universityModified.json");
                else
                    tempPath = Path.Combine(path, "university.json");

                newProfessors = SerializeUniversity.DeserializeJson(tempPath);
            }

            if (newProfessors != null)
                professors.AddRange(newProfessors);
            else
                WriteLine("Объекты для сериализации отсутствуют.");
        }

        /// <summary>
        /// метод инициализации списка преподавателей;
        /// нужен для корректной работы счетчика Professor.counter.
        /// Вызывается лишь один раз
        /// </summary>
        static void InitializeProfessorsList()
        {
            professors.Add(new("Иванов", "Иван", "Иванович", "Физика",
                new(year: 1999, month: 2, day: 9)));
            professors.Add(new("Александрова", "Александра", "Александровна", "Социология",
                new(year: 2011, month: 10, day: 16)));
            professors.Add(new("Петров", "Пётр", "Петрович", "Высшая математика",
                new(year: 2005, month: 4, day: 12)));
        }
    }
}