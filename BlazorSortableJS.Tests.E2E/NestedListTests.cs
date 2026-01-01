using NUnit.Framework;

namespace BlazorSortableJS.Tests.E2E;

[TestFixture]
public class NestedListTests : SortableTestBase
{
    [SetUp]
    public async Task TestSetUp()
    {
        await NavigateToSampleAsync("#nested-sortables-example");
        await ScrollToSectionAsync("Nested Sortables");
        await Task.Delay(500);
    }

    [Test]
    public async Task DragTopLevelItem_ReordersItems()
    {
        var lists = await GetSectionListsAsync("Nested Sortables");
        Assert.That(lists.Length, Is.GreaterThan(0), "Should find at least one list");

        // Get top-level items (nested-1 class)
        var topLevelItems = await lists[0].QuerySelectorAllAsync(".nested-1");
        Assert.That(topLevelItems.Length, Is.GreaterThanOrEqualTo(3), "Should have at least 3 top-level items");

        // Get first item text before drag
        var firstItemText = await topLevelItems[0].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");
        var secondItemText = await topLevelItems[1].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");

        // Drag first item to second position
        await DragElementAsync(topLevelItems[0], topLevelItems[1]);

        // Re-query items after drag
        topLevelItems = await lists[0].QuerySelectorAllAsync(".nested-1");
        var newFirstText = await topLevelItems[0].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");
        var newSecondText = await topLevelItems[1].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");

        // Verify reorder happened - second should now be first
        Assert.That(newFirstText, Is.EqualTo(secondItemText), "Second item should now be first");

        await Task.Delay(2000); // Time to read results
    }

    [Test]
    public async Task DragNestedItem_ReordersWithinParent()
    {
        var lists = await GetSectionListsAsync("Nested Sortables");
        Assert.That(lists.Length, Is.GreaterThan(0), "Should find at least one list");

        // Find nested items at depth 2 (nested-2 class)
        var nestedItems = await lists[0].QuerySelectorAllAsync(".nested-2");

        if (nestedItems.Length < 2)
        {
            Assert.Inconclusive("Not enough nested items to test");
            return;
        }

        // Get text before drag
        var firstNestedText = await nestedItems[0].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");
        var secondNestedText = await nestedItems[1].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");

        // Drag first nested item to second position
        await DragElementAsync(nestedItems[0], nestedItems[1]);

        // Re-query items after drag
        nestedItems = await lists[0].QuerySelectorAllAsync(".nested-2");
        var newFirstText = await nestedItems[0].EvaluateFunctionAsync<string>("el => el.childNodes[0].textContent.trim()");

        // Verify reorder happened
        Assert.That(newFirstText, Is.EqualTo(secondNestedText), "Second nested item should now be first");

        await Task.Delay(2000); // Time to read results
    }
}
