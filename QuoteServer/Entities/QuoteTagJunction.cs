namespace QuoteAppServer.Entities
{
    public class QuoteTagJunction
    {
        public int QuoteId { get; set; }
        public int TagId { get; set; }
        public Quotes Quote { get; set; }
        public QuoteTags Tag { get; set; }
    }
}
