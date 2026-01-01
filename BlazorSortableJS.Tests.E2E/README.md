# BlazorSortableJS E2E Tests

PuppeteerSharp-based end-to-end tests for the BlazorSortableJS drag-and-drop functionality.

## Prerequisites

- .NET 9.0 SDK
- Chrome/Chromium (downloaded automatically by PuppeteerSharp on first run)

## Running Tests

### 1. Start the Demo Application

In a separate terminal, start the Blazor Demo app:

```bash
cd /path/to/BlazorSortableJS
dotnet run --project BlazorSortableJS.Demo/BlazorSortableJS.Demo.csproj
```

The app will be running at `http://localhost:5076`

### 2. Run the Tests

```bash
dotnet test BlazorSortableJS.Tests.E2E/BlazorSortableJS.Tests.E2E.csproj
```

### Running Specific Tests

```bash
# Run only SimpleList tests
dotnet test --filter "FullyQualifiedName~SimpleListTests"

# Run only DisableSorting tests
dotnet test --filter "FullyQualifiedName~DisableSortingTests"

# Run a specific test
dotnet test --filter "Name=InitialState_HasThreeItems"
```

### Running with Visible Browser

To see the browser while tests run, modify `SortableTestBase.cs`:

```csharp
Browser = await Puppeteer.LaunchAsync(new LaunchOptions
{
    Headless = false  // Change from true to false
});
```

## Test Structure

```
BlazorSortableJS.Tests.E2E/
├── SortableTestBase.cs      # Base class with browser setup and helpers
├── SimpleListTests.cs       # Tests for single-list reordering
├── SharedListsTests.cs      # Tests for cross-list movement
└── DisableSortingTests.cs   # Tests for clone operations
```

## Known Limitations

HTML5 native drag-and-drop is difficult to automate with browser testing tools. Both Playwright and Puppeteer use mouse events, but Sortable.js uses native `DragEvent` objects.

**Workarounds:**
1. Add `forceFallback: true` to Sortable options (makes it use mouse events)
2. Test via JavaScript by calling internal handlers directly
3. Use bUnit for component-level testing

## Troubleshooting

### App not responding
Make sure the Demo app is running at `http://localhost:5076` before running tests.

### Browser download fails
PuppeteerSharp downloads Chromium on first run. Ensure you have internet access and sufficient disk space.

### Tests timeout
Increase the delay values in `SortableTestBase.cs` if tests are flaky due to slow rendering.
