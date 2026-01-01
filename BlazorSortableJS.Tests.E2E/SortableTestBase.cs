using System.Text.Json;
using NUnit.Framework;
using PuppeteerSharp;

namespace BlazorSortableJS.Tests.E2E;

public class SortableTestBase
{
    protected const string BaseUrl = "http://localhost:5076";
    protected IBrowser Browser = null!;
    protected IPage Page = null!;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        // Download browser if needed
        var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();

        Browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = false
        });
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await Browser.DisposeAsync();
    }

    [SetUp]
    public async Task SetUp()
    {
        Page = await Browser.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
    }

    protected async Task NavigateToSampleAsync(string anchorId)
    {
        await Page.GoToAsync($"{BaseUrl}/{anchorId}", WaitUntilNavigation.Networkidle0);
        // Wait for Sortable.js to initialize
        await Page.WaitForFunctionAsync("() => typeof Sortable !== 'undefined'");
        await Task.Delay(200);
    }

    protected async Task<List<string>> GetListItemsAsync(string listSelector)
    {
        var items = await Page.QuerySelectorAllAsync($"{listSelector} .list-group-item");
        var texts = new List<string>();
        foreach (var item in items)
        {
            var text = await item.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
            texts.Add(text);
        }
        return texts;
    }

    protected async Task<List<string>> GetJsonResultAsync(string preSelector)
    {
        var element = await Page.QuerySelectorAsync(preSelector);
        if (element == null) return new List<string>();

        var json = await element.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
        if (string.IsNullOrWhiteSpace(json)) return new List<string>();

        try
        {
            return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
        }
        catch
        {
            return new List<string>();
        }
    }

    /// <summary>
    /// Performs drag using Puppeteer's mouse API with proper steps.
    /// </summary>
    protected async Task DragElementAsync(IElementHandle source, IElementHandle target)
    {
        var sourceBox = await source.BoundingBoxAsync();
        var targetBox = await target.BoundingBoxAsync();

        if (sourceBox == null || targetBox == null)
            throw new InvalidOperationException("Could not get bounding boxes for drag elements");

        var sourceX = sourceBox.X + sourceBox.Width / 2;
        var sourceY = sourceBox.Y + sourceBox.Height / 2;
        var targetX = targetBox.X + targetBox.Width / 2;
        var targetY = targetBox.Y + targetBox.Height / 2;

        // Disable text selection to prevent interference with drag
        await Page.EvaluateFunctionAsync(@"() => {
            document.body.style.userSelect = 'none';
            document.body.style.webkitUserSelect = 'none';
        }");

        // Perform drag with mouse
        await Page.Mouse.MoveAsync((decimal)sourceX, (decimal)sourceY);
        await Task.Delay(100);
        await Page.Mouse.DownAsync();
        await Task.Delay(100);

        // Move in steps
        const int steps = 15;
        for (int i = 1; i <= steps; i++)
        {
            var x = sourceX + (targetX - sourceX) * i / steps;
            var y = sourceY + (targetY - sourceY) * i / steps;
            await Page.Mouse.MoveAsync((decimal)x, (decimal)y);
            await Task.Delay(20);
        }

        await Task.Delay(100);
        await Page.Mouse.UpAsync();
        await Task.Delay(500);

        // Re-enable text selection
        await Page.EvaluateFunctionAsync(@"() => {
            document.body.style.userSelect = '';
            document.body.style.webkitUserSelect = '';
        }");
    }

    protected async Task DragItemToPositionAsync(string listSelector, int fromIndex, int toIndex)
    {
        var items = await Page.QuerySelectorAllAsync($"{listSelector} .list-group-item");
        if (fromIndex >= items.Length || toIndex >= items.Length)
            throw new ArgumentOutOfRangeException("Index out of range");

        await DragElementAsync(items[fromIndex], items[toIndex]);
    }

    protected async Task DragItemBetweenListsAsync(string sourceListSelector, int sourceIndex, string targetListSelector, int targetIndex)
    {
        var sourceItems = await Page.QuerySelectorAllAsync($"{sourceListSelector} .list-group-item");
        var targetItems = await Page.QuerySelectorAllAsync($"{targetListSelector} .list-group-item");

        if (sourceIndex >= sourceItems.Length || targetIndex >= targetItems.Length)
            throw new ArgumentOutOfRangeException("Index out of range");

        await DragElementAsync(sourceItems[sourceIndex], targetItems[targetIndex]);
    }

    protected async Task ScrollToSectionAsync(string headingText)
    {
        await Page.EvaluateFunctionAsync($@"() => {{
            const heading = document.evaluate(""//h3[contains(text(), '{headingText}')]"", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
            if (heading) heading.scrollIntoView();
        }}");
        await Task.Delay(100);
    }

    /// <summary>
    /// Gets list groups that belong to a specific section (by heading text).
    /// This is necessary because the page has multiple sections with .list-group elements.
    /// </summary>
    protected async Task<IElementHandle[]> GetSectionListsAsync(string headingText)
    {
        // Find lists that are within the section container (between this heading and the next hr/h3)
        var lists = await Page.EvaluateFunctionHandleAsync($@"() => {{
            const heading = document.evaluate(""//h3[contains(text(), '{headingText}')]"", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;
            if (!heading) return [];

            // Find the parent that contains the sample (usually a section or the next sibling row)
            let current = heading.nextElementSibling;
            const sectionLists = [];

            // Collect all list-groups until we hit the next section separator (hr) or heading (h3)
            while (current && current.tagName !== 'HR' && current.tagName !== 'H3') {{
                const lists = current.querySelectorAll('.list-group');
                lists.forEach(l => sectionLists.push(l));
                current = current.nextElementSibling;
            }}

            return sectionLists;
        }}");

        // Convert JSHandle to ElementHandle array
        var properties = await lists.GetPropertiesAsync();
        var elements = new List<IElementHandle>();
        foreach (var prop in properties.Values)
        {
            if (prop is IElementHandle element)
            {
                elements.Add(element);
            }
        }
        return elements.ToArray();
    }
}
