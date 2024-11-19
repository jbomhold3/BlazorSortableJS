<div class="row">
    <div class="col-6">
        <h6 class="text-center fw-bold">Sortable</h6>
        <SortableWrapper OnDataChanged="@(() => InvokeAsync(StateHasChanged))">
            <Sortable TItem="string" Items="items" Class="list-group" Options="_options">
                <Template Context="item">
                    <div class="list-group-item">@item</div>
                </Template>
            </Sortable>
        </SortableWrapper>
    </div>
    <div class="col-6">
        <h6 class="text-center fw-bold">Result</h6>
        <pre class="bg-info">
            @System.Text.Json.JsonSerializer.Serialize(items, new JsonSerializerOptions
            {
                WriteIndented = true,
            });
        </pre>
    </div>
</div>
@code {
    object _options = new
    {
        animation = 150,
        ghostClass = "blue-background-class"
    };
    
    private List<string> items = new List<string>
    {
        "Item 1-1",
        "Item 1-2",
        "Item 1-3",
    };
}
