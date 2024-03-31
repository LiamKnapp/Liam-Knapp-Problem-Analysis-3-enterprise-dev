using QuoteAppServer.Entities;
using QuoteAppServer.Models;
using Microsoft.AspNetCore.Mvc;
using QuoteAppServer.Services;
using System.Diagnostics;

namespace QuoteAppServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IQuoteManager _quoteManager;

        //private readonly QuotesDB _context;

        public HomeController(ILogger<HomeController> logger, IQuoteManager quoteManager)
        {
            _logger = logger;
            _quoteManager = quoteManager;
        }

        public IActionResult Index()
        {
            var quotes = _quoteManager.GetAllQuote();

            return View(quotes);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
