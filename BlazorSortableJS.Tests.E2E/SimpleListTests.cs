using NUnit.Framework;

namespace BlazorSortableJS.Tests.E2E;

[TestFixture]
public class SimpleListTests : SortableTestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await NavigateToSampleAsync("#simple-list-example");
        await ScrollToSectionAsync("Simple list example");
        await Task.Delay(500);
    }

    [Test]
    public async Task DragFirstToLast_ReordersItems()
    {
        var lists = await GetSectionListsAsync("Simple list example");
        Assert.That(lists.Length, Is.EqualTo(1), "Should find exactly 1 list in Simple list section");

        var firstList = lists[0];
        var items = await firstList.QuerySelectorAllAsync(".list-group-item");

        // Drag first item to last position
        await DragElementAsync(items[0], items[2]);

        // Re-query items after drag
        items = await firstList.QuerySelectorAllAsync(".list-group-item");
        var texts = new List<string>();
        foreach (var item in items)
        {
            var text = await item.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
            texts.Add(text);
        }

        // After drag, order should be: 1-2, 1-3, 1-1
        Assert.That(texts[0], Is.EqualTo("Item 1-2"));
        Assert.That(texts[2], Is.EqualTo("Item 1-1"));

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task DragSecondToFirst_ReordersItems()
    {
        var lists = await GetSectionListsAsync("Simple list example");
        var firstList = lists[0];
        var items = await firstList.QuerySelectorAllAsync(".list-group-item");

        // Drag second item to first position
        await DragElementAsync(items[1], items[0]);

        // Re-query items after drag
        items = await firstList.QuerySelectorAllAsync(".list-group-item");
        var texts = new List<string>();
        foreach (var item in items)
        {
            var text = await item.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
            texts.Add(text);
        }

        // After drag, order should be: 1-2, 1-1, 1-3
        Assert.That(texts[0], Is.EqualTo("Item 1-2"));
        Assert.That(texts[1], Is.EqualTo("Item 1-1"));
        Assert.That(texts[2], Is.EqualTo("Item 1-3"));

        await Task.Delay(2000); // Time to read results
    }
}
