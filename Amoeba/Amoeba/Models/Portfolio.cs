namespace Amoeba.Models
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string Client { get; set; } = null!;
        public DateTime ProjectDate { get; set; }
        public string ProjectUrl { get; set; } = null!;
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Detail { get; set; }
      
    }
}
