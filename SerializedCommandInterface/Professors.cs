using System.Reflection.Metadata;

namespace SerializedCommandInterface
{
    [Serializable]
    public class Professors
    {
        public Professors() { }

        public List<Professor> professorsList { get; set; } = new();
    }
}
