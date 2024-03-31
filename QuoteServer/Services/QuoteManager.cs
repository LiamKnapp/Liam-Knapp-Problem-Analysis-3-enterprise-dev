using Microsoft.EntityFrameworkCore;
using QuoteAppServer.Entities;

namespace QuoteAppServer.Services
{
    public class QuoteManager : IQuoteManager
    {
        private readonly QuotesDB _context;

        public QuoteManager(QuotesDB context)
        {
            _context = context;
        }

        // Get all quotes
        public ICollection<Quotes> GetAllQuote()
        {
            return _context.Quotes.ToList();
        }

        public ICollection<int> GetAllQuoteIDs()
        {
            return _context.Quotes
                           .Select(q => q.QuoteId)  // Select only the QuoteId property
                           .ToList();
        }

        // Get all tags
        public ICollection<QuoteTags> GetAllTags()
        {
            return _context.QuoteTags.ToList();
        }


        // Get quotes by likes sorted in descending order
        public List<Quotes> GetQuotesByLikes(int likes)
        {
            return _context.Quotes
                            .Where(q => q.Likes >= likes)
                            .OrderByDescending(q => q.Likes)  // Sort by likes in descending order
                            .ToList();
        }

        // Get quotes by tag ID
        public List<int> GetQuotesByTagId(int tagId)
        {
            var quoteIds = _context.QuoteTagJunctions
                                .Where(j => j.TagId == tagId)
                                .Select(j => j.QuoteId)
                                .ToList();
            return quoteIds;
        }


        // Get tag by ID
        public QuoteTags GetTagById(string tagId)
        {
            int id;
            if (!int.TryParse(tagId, out id))
                throw new ArgumentException("Invalid tag ID");

            return _context.QuoteTags.FirstOrDefault(t => t.TagId == id);
        }

        // Get tags From quotes
        public List<QuoteTags> GetTagsFromQuote(int quoteId)
        {
            return _context.QuoteTagJunctions
                .Where(j => j.QuoteId == quoteId)
                .Select(j => j.Tag)
                .ToList();
        }


        // Add new quote
        public int AddNewQuote(Quotes quote)
        {
            _context.Quotes.Add(quote);
            _context.SaveChanges();
            return quote.QuoteId;
        }


        // Add new tag
        public int AddNewTag(QuoteTags tag)
        {
            _context.QuoteTags.Add(tag);
            _context.SaveChanges();
            return tag.TagId;
        }

        // Add tag to quote
        public void AddTagToQuote(int quoteId, int tagId)
        {
            var quote = _context.Quotes.Find(quoteId);
            var tag = _context.QuoteTags.Find(tagId);

            if (quote != null && tag != null)
            {
                // Check if the tag is already associated with the quote
                if (!_context.QuoteTagJunctions.Any(j => j.QuoteId == quoteId && j.TagId == tagId))
                {
                    // Create a new QuoteTagJunction object
                    var junction = new QuoteTagJunction
                    {
                        QuoteId = quoteId,
                        TagId = tagId
                    };

                    // Add the junction to the context
                    _context.QuoteTagJunctions.Add(junction);

                    // Save changes to the database
                    _context.SaveChanges();
                }
            }
        }

        //Increment the likes on a quote
        public void UpdateLikes(int quoteId)
        {
            var quote = _context.Quotes.Find(quoteId);
            if (quote != null)
            {
                quote.Likes++;
                _context.SaveChanges();
            }
        }

        // Update existing quote
        public void UpdateQuote(Quotes quote)
        {
            _context.Entry(quote).State = EntityState.Modified;
            _context.SaveChanges();
        }


        // Update existing tag
        public void UpdateTag(QuoteTags tag)
        {
            _context.Entry(tag).State = EntityState.Modified;
            _context.SaveChanges();
        }


        // Delete quote by ID
        public void DeleteQuoteById(int id)
        {
            var quote = _context.Quotes.Find(id);
            if (quote != null)
            {
                _context.Quotes.Remove(quote);
                _context.SaveChanges();
            }
        }


        // Delete tag by ID
        public void DeleteTagById(int id)
        {
            var tag = _context.QuoteTags.Find(id);
            if (tag != null)
            {
                _context.QuoteTags.Remove(tag);
                _context.SaveChanges();
            }
        }


        // Remove tag from quote
        public void RemoveTagFromQuote(int quoteId, int tagId)
        {
            var quote = _context.Quotes.Find(quoteId);
            var tag = _context.QuoteTags.Find(tagId);
            if (quote != null && tag != null)
            {
                var junction = _context.QuoteTagJunctions.FirstOrDefault(j => j.QuoteId == quoteId && j.TagId == tagId);
                if (junction != null)
                {
                    _context.QuoteTagJunctions.Remove(junction);
                    _context.SaveChanges();
                }
            }
        }

    }
}
