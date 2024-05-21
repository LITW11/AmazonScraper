using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace AmazonScraper.Web.Services
{
    public class AmazonItem
    {
        public string Title { get; set; }
        public decimal? Price { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
    }

    public class Scraper
    {
        public List<AmazonItem> Scrape(string query)
        {
            var html = GetAmazonHtml(query);
            var parser = new HtmlParser();
            var document = parser.ParseDocument(html);

            var resultDivs = document.QuerySelectorAll(".s-result-item");

            return resultDivs.Select(div => ParseItem(div)).Where(i => i != null).ToList();
        }

        private string GetAmazonHtml(string query)
        {
            var url = $"https://amazon.com/s?k={query}";
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate,
                UseCookies = true
            };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/124.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US");
            return client.GetStringAsync(url).Result;
        }

        private AmazonItem ParseItem(IElement div)
        {
            var amazonItem = new AmazonItem();
            var titleElement = div.QuerySelector("span.a-size-medium");
            if (titleElement == null)
            {
                return null;
            }
            amazonItem.Title = titleElement.TextContent;

            var priceWhole = div.QuerySelector("span.a-price-whole");
            var priceFraction = div.QuerySelector("span.a-price-fraction");
            if (priceWhole != null && priceFraction != null)
            {
                var price = $"{priceWhole.TextContent}{priceFraction.TextContent}".Replace("$", "");
                amazonItem.Price = decimal.Parse(price);
            }

            var imageElement = div.QuerySelector("img.s-image");
            if (imageElement != null)
            {
                var src = imageElement.Attributes["src"].Value;
                amazonItem.Image = src;
            }

            var anchorTag = div.QuerySelector("a.a-link-normal");
            if (anchorTag != null)
            {
                amazonItem.Url = $"https://amazon.com{anchorTag.Attributes["href"].Value}";
            }

            return amazonItem;
        }
    }
}
