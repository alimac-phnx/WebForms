namespace WebForms.Models
{
    public class Template
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TopicId { get; set; } // Например: "Education", "Quiz", "Other"

        public string? ImageUrl { get; set; } // URL загруженного изображения

        public List<Tag> Tags { get; set; } = new List<Tag>(); // Список тегов

        public int CreatedByUserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>(); // no migration yet -- formtemplate pair?
    }
}
