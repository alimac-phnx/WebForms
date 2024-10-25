namespace WebForms.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Type { get; set; }

        public bool IsVisible { get; set; }

        public int TemplateId { get; set; }
    }
}