namespace WebForms.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int QuestionId { get; set; }

        public int FormId { get; set; }
    }
}