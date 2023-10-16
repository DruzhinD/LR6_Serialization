using static System.Console;
using System.Text.Json;

namespace WorkingWithJson
{
    internal class Program
    {
        static Book csharp10 = new(title: "C# 10 and .Net 6 - Modern Cross-platform Development")
        {
            Author = "Mark J Price",
            PublishDate = new(2021, 11, 9),
            Pages = 823,
            Created = DateTimeOffset.UtcNow,
        };

        static void Main(string[] args)
        {
            JsonSerialization();
        }

        static void JsonSerialization()
        {
            JsonSerializerOptions options = new()
            {
                IncludeFields = true, //включает в себя все поля
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\", "book.json");
            filePath = Path.GetFullPath(filePath);

            using (Stream fileStream = File.Create(filePath))
            {
                JsonSerializer.Serialize<Book>(
                    utf8Json: fileStream, value: csharp10, options);
            }

            WriteLine("Written {0:N0} bytes of JSON to {1}",
                arg0: new FileInfo(filePath).Length,
                arg1: filePath);
            WriteLine();

            //отображаем сериализованный граф объектов
            WriteLine(File.ReadAllText(filePath));
        }
    }
}