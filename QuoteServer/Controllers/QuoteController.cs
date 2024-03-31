using QuoteAppServer.Models;
using Microsoft.AspNetCore.Mvc;
using QuoteAppServer.Entities;
using QuoteAppServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuoteAppServer.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IQuoteManager _quoteManager;

        public QuoteController(IQuoteManager quoteManager)
        {
            _quoteManager = quoteManager;
        }

        //-----------------------------------------------------------------------------------------------
        // Get Items
        //-----------------------------------------------------------------------------------------------

        // Get all Quotes
        [HttpGet("/Quotes")]
        public IActionResult GetQuotes()
        {
            var quotes = _quoteManager.GetAllQuote()
                .Select(q => new Quotes
                {
                    QuoteId = q.QuoteId,
                    Quote = q.Quote,
                    Author = q.Author,
                    Likes = q.Likes.GetValueOrDefault(),
                })
                .ToList();

            return Json(quotes);
        }

        // Get Quote By ID
        [HttpGet("/Quotes/{id}")]
        public IActionResult GetQuote(int id)
        {
            var quote = _quoteManager.GetAllQuote().FirstOrDefault(q => q.QuoteId == id);
            if (quote == null)
            {
                return NotFound();
            }

            var quotes = new Quotes
            {
                QuoteId = quote.QuoteId,
                Quote = quote.Quote,
                Author = quote.Author,
                Likes = quote.Likes.GetValueOrDefault(),
            };

            return Json(quotes);
        }

        [HttpGet("/Quotes/ids/")]
        public IActionResult GetAllQuoteIds()
        {
            var quoteIds = _quoteManager.GetAllQuoteIDs();
            return Ok(quoteIds);
        }

        // Get quotes by tag ID
        [HttpGet("/Quotes/Tag/{tagId}")]
        public IActionResult GetQuoteIdsByTagId(int tagId)
        {
            var quoteIds = _quoteManager.GetQuotesByTagId(tagId);
            return Json(quoteIds);
        }

        // Get quotes by likes
        [HttpGet("/Quotes/Likes")]
        public IActionResult GetQuotesByLikes(int likes)
        {
            var quotes = _quoteManager.GetQuotesByLikes(likes)
                .Select(q => new Quotes
                {
                    QuoteId = q.QuoteId,
                    Quote = q.Quote,
                    Author = q.Author,
                    Likes = q.Likes.GetValueOrDefault(),
                })
                .ToList();

            return Json(quotes);
        }

        // Get all Tags
        [HttpGet("/Tags")]
        public IActionResult GetTags()
        {
            var tags = _quoteManager.GetAllTags()
                 .Select(t => new QuoteTags
                 {
                     TagId = t.TagId,
                     TagName = t.TagName,
                 })
                 .ToList();

            return Json(tags);
        }

        // Get Tag By ID
        [HttpGet("/Tags/{id}")]
        public IActionResult GetTag(int id)
        {
            var tags = _quoteManager.GetAllTags().FirstOrDefault(q => q.TagId == id);
            if (tags == null)
            {
                return NotFound();
            }

            var tag = new QuoteTags
            {
                TagId = tags.TagId,
                TagName = tags.TagName
            };

            return Ok(tags);
        }

        // Get tags associated with a quote
        [HttpGet("/Quotes/{quoteId}/Tags")]
        public IActionResult GetTagsFromQuote(int quoteId)
        {
            var tags = _quoteManager.GetTagsFromQuote(quoteId);
            return Json(tags);
        }

        //-----------------------------------------------------------------------------------------------
        // Post Items
        //-----------------------------------------------------------------------------------------------

        // Add new quote
        [HttpPost("/Quotes/add")]
        public IActionResult AddQuote([FromBody] Quotes quotes)
        {
            if (quotes == null)
                return BadRequest();

            var quote = new Quotes
            {
                Quote = quotes.Quote,
                Author = quotes.Author,
                Likes = 0 // New quotes start with 0 likes
            };

            _quoteManager.AddNewQuote(quote);

            return CreatedAtAction(nameof(GetQuote), new { id = quote.QuoteId }, quote);
        }

        // Add new tag
        [HttpPost("/Tags/add")]
        public IActionResult AddTag([FromBody] QuoteTags tag)
        {
            if (tag == null)
                return BadRequest();

            _quoteManager.AddNewTag(tag);

            return CreatedAtAction(nameof(GetTag), new { id = tag.TagId }, tag);
        }

        // Add new tag to quote
        [HttpPost("/Quotes/Tags/add/{quoteid}/{tagid}")]
        public IActionResult AddTagToQuote(int quoteid, int tagid)
        {

            _quoteManager.AddTagToQuote(quoteid, tagid);

            return Ok();
        }

        //-----------------------------------------------------------------------------------------------
        // Update Items
        //-----------------------------------------------------------------------------------------------

        // Increment Likes
        [HttpPut("/Quotes/{id}/Likes")]
        public IActionResult UpdateLikes(int id)
        {
            _quoteManager.UpdateLikes(id);

            return NoContent();
        }

        // Update the quote
        [HttpPut("/Quotes/{id}")]
        public IActionResult UpdateQuote(int id, [FromBody] Quotes quotes)
        {
            var existingQuote = _quoteManager.GetAllQuote().FirstOrDefault(q => q.QuoteId == id);
            if (existingQuote == null)
                return NotFound();

            existingQuote.Quote = quotes.Quote;
            existingQuote.Author = quotes.Author;

            _quoteManager.UpdateQuote(existingQuote);

            return NoContent();
        }


        // Update existing tag
        [HttpPut("/Tags/{id}")]
        public IActionResult UpdateTag(int id, [FromBody] QuoteTags tag)
        {
            var existingTag = _quoteManager.GetAllTags().FirstOrDefault(q => q.TagId == id);
            if (existingTag == null)
                return NotFound();

            existingTag.TagName = tag.TagName;

            _quoteManager.UpdateTag(existingTag);

            return NoContent();
        }

        //-----------------------------------------------------------------------------------------------
        // Delete Items
        //-----------------------------------------------------------------------------------------------


        // Delete a Quote
        [HttpDelete("/Quotes/{id}")]
        public IActionResult DeleteQuote(int id)
        {
            _quoteManager.DeleteQuoteById(id);
            return Ok();
        }


        // Delete a Tag
        [HttpDelete("/Tags/{id}")]
        public IActionResult DeleteTag(int id)
        {
            _quoteManager.DeleteTagById(id);
            return Ok();
        }


        // Delete Tag from Quote
        [HttpDelete("/Quotes/{quoteid}/Tag/{tagid}")]
        public IActionResult DeleteTagFromQuote(int quoteid, int tagid)
        {
            _quoteManager.RemoveTagFromQuote(quoteid, tagid);


            return NoContent();
        }
    }
}
