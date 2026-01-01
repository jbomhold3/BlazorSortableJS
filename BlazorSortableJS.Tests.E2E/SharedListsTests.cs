using NUnit.Framework;

namespace BlazorSortableJS.Tests.E2E;

[TestFixture]
public class SharedListsTests : SortableTestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await NavigateToSampleAsync("#shared-lists");
        await ScrollToSectionAsync("Shared lists");
        await Task.Delay(500);
    }

    [Test]
    public async Task DragBetweenLists_MovesItem()
    {
        var lists = await GetSectionListsAsync("Shared lists");
        Assert.That(lists.Length, Is.EqualTo(2), "Should find exactly 2 lists in Shared lists section");

        var list1 = lists[0];
        var list2 = lists[1];

        var list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        var list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        Assert.That(list1Items.Length, Is.EqualTo(3), "List 1 should have 3 items initially");
        Assert.That(list2Items.Length, Is.EqualTo(3), "List 2 should have 3 items initially");

        // Drag first item from list1 to first position in list2
        await DragElementAsync(list1Items[0], list2Items[0]);

        // Verify list counts
        list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        Assert.That(list1Items.Length, Is.EqualTo(2), "List 1 should have 2 items after move");
        Assert.That(list2Items.Length, Is.EqualTo(4), "List 2 should have 4 items after move");

        // Verify exact order in list2: Item 1-1 should be at position 0
        var list2Texts = new List<string>();
        foreach (var item in list2Items)
        {
            var text = await item.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
            list2Texts.Add(text);
        }
        Assert.That(list2Texts[0], Is.EqualTo("Item 1-1"), "Item 1-1 should be first in List 2");

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task DragSecondItemToList2_MovesItem()
    {
        var lists = await GetSectionListsAsync("Shared lists");
        var list1 = lists[0];
        var list2 = lists[1];

        var list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        var list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        // Drag second item from list1 to end of list2
        await DragElementAsync(list1Items[1], list2Items[2]);

        // Verify list counts
        list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        Assert.That(list1Items.Length, Is.EqualTo(2), "List 1 should have 2 items after move");
        Assert.That(list2Items.Length, Is.EqualTo(4), "List 2 should have 4 items after move");

        // Verify exact order in list2: Item 1-2 should be at position 2 (before last)
        var list2Texts = new List<string>();
        foreach (var item in list2Items)
        {
            var text = await item.EvaluateFunctionAsync<string>("el => el.textContent.trim()");
            list2Texts.Add(text);
        }
        Assert.That(list2Texts[2], Is.EqualTo("Item 1-2"), "Item 1-2 should be at position 2 in List 2");

        await Task.Delay(2000); // Time to read results
    }
}
