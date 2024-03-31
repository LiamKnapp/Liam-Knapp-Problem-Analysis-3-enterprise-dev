using System.ComponentModel.DataAnnotations;

namespace QuoteAppServer.Entities
{
    public class QuoteTags
    {
        [Key]
        public int TagId { get; set; }
        public string? TagName { get; set; }
        public ICollection<Quotes>? Quote { get; set; }
    }
}
