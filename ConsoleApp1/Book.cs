using System.Text.Json.Serialization;

namespace WorkingWithJson
{
    public class Book
    {
        public Book(string title)
        {
            Title = title;
        }

        public string Title { get; set; }
        public string? Author { get; set; }

        [JsonInclude]
        public DateTime PublishDate; //DateOnly не поддерживается
        [JsonInclude]
        public DateTimeOffset Created;

        public ushort Pages;
    }
}