namespace WebForms.Models
{
    public class Template
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; }

        public string? ImageUrl { get; set; }

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
