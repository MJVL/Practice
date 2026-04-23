using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

string? NormalizeUrl(string url)
{
    // if no protocol, we'll force one
    if (!url.StartsWith("http")) {
        url = "https://" + url;
    }

    // let's try parsing this
    if (Uri.TryCreate(url, UriKind.Absolute, out Uri? parsedUrl))
    {
        return "https://" + parsedUrl.Host;
    }
    return null;
}

HashSet<string> MakeUnique(IEnumerable<string> urls)
{
    return urls
        .Select(NormalizeUrl)
        .Where(u => !string.IsNullOrWhiteSpace(u))
        .Select(u => u!)
        .ToHashSet();
}

async Task ScanTargets(IEnumerable<string> targets)
{
    targets = MakeUnique(targets);

    HttpClient client = new HttpClient();
    foreach (string url in targets)
    {

        try
        {
            Console.WriteLine($"Scanning {url}");
            var resp = await client.GetAsync(url);
            Console.WriteLine($"Resp={resp.StatusCode}");
        } catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex}");
        }
    }
}

// this is "one" url from our pov, for now at least
var targets = new string[]{"https://www.google.com", "http://www.google.com", "www.google.com", "www.google.com?s=5", "www.google.com#hello!", "www.youtube.com", "www.github.com", "www.tryhackme.com"};
foreach (string t in targets)
{
    Console.Out.WriteLine(NormalizeUrl(t));
}

var sw = System.Diagnostics.Stopwatch.StartNew();
Console.Out.WriteLine(MakeUnique(targets).Count);
await ScanTargets(targets);
sw.Stop();
Console.Out.WriteLine($"Took {sw.Elapsed.Seconds} seconds ({sw.Elapsed.Milliseconds} milliseconds)");