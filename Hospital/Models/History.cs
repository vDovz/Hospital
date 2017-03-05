namespace Hospital.Models
{
    public class History
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string  Description { get; set; }

        public Patient Patient { get; set; }
    }
}