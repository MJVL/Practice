using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

string? NormalizeUrl(string url)
{
    // if no protocol, we'll force one
    if (!url.StartsWith("http"))
    {
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


// crude version, loop and go
async Task ScanTargetsOne(IEnumerable<string> targets)
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex}");
        }
    }
}

// better: we now do this in parallel
async Task ScanTargetsTwo(IEnumerable<string> targets, int maxConcurrency = 10)
{
    targets = MakeUnique(targets);

    HttpClient client = new HttpClient();
    var tasks = new List<Task>();

    foreach (string url in targets)
    {
        tasks.Add(Task.Run(async () =>
        {
            try
            {
                Console.WriteLine($"Scanning {url}");
                var resp = await client.GetAsync(url);
                Console.WriteLine($"Resp={resp.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex}");
            }
        }));
    }
    await Task.WhenAll(tasks);
}

// better: we now actually do this in parallel, with control
async Task ScanTargetsThree(IEnumerable<string> targets, int maxConcurrency = 10)
{
    targets = MakeUnique(targets);

    HttpClient client = new HttpClient();
    SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
    var tasks = new List<Task>();

    foreach (string url in targets)
    {
        tasks.Add(Task.Run(async () =>
        {
            await semaphore.WaitAsync();
            try
            {
                Console.WriteLine($"Scanning {url}");
                var resp = await client.GetAsync(url);
                Console.WriteLine($"Resp={resp.StatusCode}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex}");
            }
            finally
            {
                semaphore.Release();
            }
        }));
    }
    await Task.WhenAll(tasks);
}

// better: no need for task.Run, async is really already making a new unit of work
async Task ScanTargetsFour(IEnumerable<string> targets, int maxConcurrency = 10)
{
    targets = MakeUnique(targets);

    HttpClient client = new HttpClient();
    SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency);
    var tasks = new List<Task>();

    foreach (string url in targets)
    {
        tasks.Add(ScanUrl(url));
    }
    await Task.WhenAll(tasks);

    async Task ScanUrl(string url)
    {
        await semaphore.WaitAsync();
        try
        {
            Console.WriteLine($"Scanning {url}");
            var resp = await client.GetAsync(url);
            Console.WriteLine($"Resp={resp.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex}");
        }
        finally
        {
            semaphore.Release();
        }
    }
}

// next up, do this more gracefully, more complex logic or crawling bfs and queuing these correctly

// this is "one" url from our pov, for now at least
var targets = new string[] { "https://www.google.com", "http://www.google.com", "www.google.com", "www.google.com?s=5", "www.google.com#hello!", "www.youtube.com", "www.github.com", "www.tryhackme.com" };
foreach (string t in targets)
{
    Console.Out.WriteLine(NormalizeUrl(t));
}

var sw = System.Diagnostics.Stopwatch.StartNew();
Console.Out.WriteLine(MakeUnique(targets).Count);
await ScanTargetsFour(targets);
sw.Stop();
Console.Out.WriteLine($"Took {sw.Elapsed.Seconds} seconds ({sw.Elapsed.Milliseconds} milliseconds)");