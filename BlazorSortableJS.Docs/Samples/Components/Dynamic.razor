<div class="row">
    <div class="col-6">
        <h6 class="text-center fw-bold">Sortable</h6>
        <SortableWrapper OnDataChanged="@(() => InvokeAsync(StateHasChanged))" @ref="_wrapperRef">
            @foreach (var dropZone in sortables)
            {
                <Sortable TItem="string" Items="@dropZone.Items" Class="list-group mb-3" Options="_options">
                    <Template Context="item"> 
                        <div class="list-group-item">@item</div>
                    </Template>
                </Sortable>
            }
        </SortableWrapper>
        <div class="row">
            <div class="col-12 text-center">
                <button class="btn btn-primary" @onclick="AddDropZone">Add DropZone</button>
                <button class="btn btn-danger" @onclick="RemoveDropZone">Remove DropZone</button>
            </div>
        </div>
    </div>
    <div class="col-6">
        <h6 class="text-center fw-bold">Result</h6>
        @foreach (var sortable in sortables)
        {
            <div class="list-group mb-3">
                @foreach (var item in sortable.Items)
                {
                    <div class="list-group-item">@item</div>
                }
            </div>
        }
    </div>
</div>

@code {
    object _options = new
    {
        group = "example2",
        animation = 150,
        ghostClass = "blue-background-class"
    };
    private SortableWrapper _wrapperRef;
    private List<SortableInfo> sortables = new List<SortableInfo>
    {
        new SortableInfo { Items = new List<string> { "Item 1.1", "Item 1.2", "Item 1.3" } },
        new SortableInfo { Items = new List<string> { "Item 2.1", "Item 2.2", "Item 2.3" } }
    };


    private async Task AddDropZone()
    {
        var groupNumber = sortables.Count + 1;
        sortables.Add(new SortableInfo { Items = new List<string> { $"Item {groupNumber}.1", $"Item {groupNumber}.2", $"Item {groupNumber}.3" } });
        await _wrapperRef.RefreshAsync();
    }

    private async Task RemoveDropZone()
    {
        if (sortables.Count > 0)
        {
            sortables.RemoveAt(sortables.Count - 1);
        }
        await _wrapperRef.RefreshAsync();
    }

    private class SortableInfo
    {
        public List<string> Items { get; set; } = new List<string>();
    }
}