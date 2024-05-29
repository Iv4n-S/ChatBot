namespace ChatBot.Models
{
    public class MemoryResponseDto
    {
        public string Text { get; set; }
        public float Relevance { get; set; }

        public MemoryResponseDto(string text, float relevance) { 
            this.Text = text;
            this.Relevance = relevance;
        }
    }
}
