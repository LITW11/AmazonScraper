using AmazonScraper.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AmazonScraper.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmazonScraping : ControllerBase
    {
        [Route("Scrape")]
        public List<AmazonItem> Scrape(string query)
        {
            var scaper = new Scraper();
            return scaper.Scrape(query);
        }
    }
}
