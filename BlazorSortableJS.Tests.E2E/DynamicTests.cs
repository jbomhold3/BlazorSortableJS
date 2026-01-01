using NUnit.Framework;

namespace BlazorSortableJS.Tests.E2E;

[TestFixture]
public class DynamicTests : SortableTestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await NavigateToSampleAsync("#dynamic-example");
        await ScrollToSectionAsync("Dynamic Example");
        await Task.Delay(500);
    }

    [Test]
    public async Task DragBetweenDynamicLists_MovesItem()
    {
        var lists = await GetSectionListsAsync("Dynamic Example");
        Assert.That(lists.Length, Is.GreaterThanOrEqualTo(2), "Should have at least 2 lists initially");

        var list1 = lists[0];
        var list2 = lists[1];

        var list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        var list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        var initialList1Count = list1Items.Length;
        var initialList2Count = list2Items.Length;

        // Get first item text
        var draggedItemText = await list1Items[0].EvaluateFunctionAsync<string>("el => el.textContent.trim()");

        // Drag first item from list1 to list2
        await DragElementAsync(list1Items[0], list2Items[0]);

        // Re-query items
        list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        list2Items = await list2.QuerySelectorAllAsync(".list-group-item");

        Assert.That(list1Items.Length, Is.EqualTo(initialList1Count - 1), "List 1 should have one less item");
        Assert.That(list2Items.Length, Is.EqualTo(initialList2Count + 1), "List 2 should have one more item");

        // Verify the item is now first in list2
        var firstList2Text = await list2Items[0].EvaluateFunctionAsync<string>("el => el.textContent.trim()");
        Assert.That(firstList2Text, Is.EqualTo(draggedItemText), "Dragged item should be first in List 2");

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task AddDropZone_CreatesNewList()
    {
        var lists = await GetSectionListsAsync("Dynamic Example");
        var initialListCount = lists.Length;

        // Find and click the Add DropZone button
        var addButton = await Page.QuerySelectorAsync("button.btn-primary");
        Assert.That(addButton, Is.Not.Null, "Should find Add DropZone button");

        await addButton.ClickAsync();
        await Task.Delay(500); // Wait for Blazor to update

        // Re-query lists (note: both sortable and result columns have lists, so count increases by 2)
        lists = await GetSectionListsAsync("Dynamic Example");
        Assert.That(lists.Length, Is.EqualTo(initialListCount + 2), "Should have two more lists after adding (sortable + result)");

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task AddAndDragToNewList_MovesItem()
    {
        // Add a new drop zone first
        var addButton = await Page.QuerySelectorAsync("button.btn-primary");
        await addButton.ClickAsync();
        await Task.Delay(500);

        var lists = await GetSectionListsAsync("Dynamic Example");
        Assert.That(lists.Length, Is.GreaterThanOrEqualTo(3), "Should have at least 3 lists after adding");

        var list1 = lists[0];
        var list3 = lists[2]; // The newly added list

        var list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        var list3Items = await list3.QuerySelectorAllAsync(".list-group-item");

        var initialList1Count = list1Items.Length;
        var initialList3Count = list3Items.Length;

        // Drag from list1 to the new list3
        await DragElementAsync(list1Items[0], list3Items[0]);

        // Re-query items
        list1Items = await list1.QuerySelectorAllAsync(".list-group-item");
        list3Items = await list3.QuerySelectorAllAsync(".list-group-item");

        Assert.That(list1Items.Length, Is.EqualTo(initialList1Count - 1), "List 1 should have one less item");
        Assert.That(list3Items.Length, Is.EqualTo(initialList3Count + 1), "List 3 should have one more item");

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task RemoveDropZone_RemovesList()
    {
        var lists = await GetSectionListsAsync("Dynamic Example");
        var initialListCount = lists.Length;

        // Find and click the Remove DropZone button
        var removeButton = await Page.QuerySelectorAsync("button.btn-danger");
        Assert.That(removeButton, Is.Not.Null, "Should find Remove DropZone button");

        await removeButton.ClickAsync();
        await Task.Delay(500); // Wait for Blazor to update

        // Re-query lists (note: both sortable and result columns have lists, so count decreases by 2)
        lists = await GetSectionListsAsync("Dynamic Example");
        Assert.That(lists.Length, Is.EqualTo(initialListCount - 2), "Should have two less lists after removing (sortable + result)");

        await Task.Delay(2000); // Time to read results
    }
}
