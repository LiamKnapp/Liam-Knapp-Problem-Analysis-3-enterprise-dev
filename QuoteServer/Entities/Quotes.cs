using System.ComponentModel.DataAnnotations;

namespace QuoteAppServer.Entities
{
    public class Quotes
    {
        [Key]
        public int QuoteId { get; set; }
        [Required]
        public string? Quote { get; set; }

        public string? Author { get; set; }
        public int? Likes { get; set; }

        public ICollection<QuoteTags>? QuoteTags { get; set; }
    }
}
