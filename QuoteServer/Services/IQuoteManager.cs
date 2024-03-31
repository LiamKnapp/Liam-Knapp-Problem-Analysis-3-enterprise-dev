using QuoteAppServer.Entities;

namespace QuoteAppServer.Services
{
    public interface IQuoteManager
    {

        //Get all items
        public ICollection<Quotes> GetAllQuote();

        public ICollection<int> GetAllQuoteIDs();

        public ICollection<QuoteTags> GetAllTags();


        //Get Items by Filtered search
        public List<Quotes> GetQuotesByLikes(int likes);


        //Get Items By ID
        public List<int> GetQuotesByTagId(int tagId);

        public List<QuoteTags> GetTagsFromQuote(int quoteId);


        //Post New Items
        public int AddNewQuote(Quotes quote);
        public int AddNewTag(QuoteTags tag);
        public void AddTagToQuote(int quoteId, int tagId);


        //Put Items
        public void UpdateLikes(int likes);
        public void UpdateQuote(Quotes quote);
        public void UpdateTag(QuoteTags tag);


        //Delete Items
        public void DeleteQuoteById(int id);
        public void DeleteTagById(int id);
        public void RemoveTagFromQuote(int quoteId, int tagId);


    }
}

