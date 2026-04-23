using System.Threading.Tasks;
using AngleSharp.Html.Parser;

public class BFSCrawler : Crawler {

    public BFSCrawler(Uri startUri) : base(startUri) {}

    public BFSCrawler(Uri startUri, HttpClient httpClient, HtmlParser htmlParser) : base(startUri, httpClient, htmlParser) {}

    public override async Task Crawl()
    {
        var page = await ParsePage(_startUri);
        // todo,
        if (page != null) {
            var links = page.QuerySelectorAll("a[href]").Select(a => a.GetAttribute("href")).Where(href => !string.IsNullOrWhiteSpace(href));
            foreach (var link in links)
                System.Console.WriteLine(link);
        }
    }
}