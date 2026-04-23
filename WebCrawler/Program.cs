using System.Threading.Tasks;

public class Program
{
    public static async Task Main()
    {
        var crawler = new BFSCrawler(new Uri("https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-dotnet-cli"));
        await crawler.Crawl();
    }
}