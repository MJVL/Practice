using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

public abstract class Crawler
{
    private readonly HttpClient _httpClient;
    private readonly HtmlParser _htmlParser;
    protected readonly Uri _startUri;

    public Crawler(Uri startUri) : this(startUri, new HttpClient(), new HtmlParser()) {}

    public Crawler(Uri startUri, HttpClient httpClient, HtmlParser htmlparser)
        => (_startUri, _httpClient, _htmlParser) = (startUri, httpClient, htmlparser);

    protected async Task<IHtmlDocument?> ParsePage(Uri uri)
    {
        try {
            using HttpResponseMessage? response = await _httpClient.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
                return null;

            // TOOD: content-type filtering

            string html = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(html))
                return null;

            //https://programmersought.com/article/53641047060/
            return await _htmlParser.ParseDocumentAsync(html);
        }
        catch (Exception)
        {
            return null;
        }
    }

    //public IEnumerable<Uri> FindLinks(string content)
    //{
    //    var
    //    return null;
    //}

    public abstract Task Crawl();
}