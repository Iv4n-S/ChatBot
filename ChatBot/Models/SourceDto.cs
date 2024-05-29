namespace ChatBot.Models
{
    public class SourceDto
    {
        public string FileId { get; set; }
        public int PartitionNumber { get; set; }
        public string Text { get; set; }
        public float Relevance { get; set; }
    }
}
