using ChatBot.Models;
using System.Text.Json;

namespace ChatBot
{
    public static class SourcesList
    {
        public static List<SourceDto> Sources { get; set; } = new List<SourceDto>();

        public static void AddSource(SourceDto source)
        {
            Sources.Add(source);
        }
        public static void SetSources(List<SourceDto> sources)
        {
            Sources.AddRange(sources);
        }

        public static IEnumerable<SourceDto> GetSources()
        {
            return Sources;
        }

        public static void ResetSources()
        {
            Sources = new List<SourceDto>();
        }
    }
}
