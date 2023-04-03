<Sortable TItem="NestedModel" Items="Items" Class="list-group" Options="_options">
    <Template Context="item">
        <div class="list-group-item nested-@Depth">
            @item.Data
            @if (item.Children.Count > 0)
            {
                <NestedRecursive Items="item.Children" Depth="@(Depth + 1)"/>
            }
        </div>
    </Template>
</Sortable>

@code {
    object _options = new
    {
        group = "nestedexample",
        animation = 150,
        ghostClass = "blue-background-class"
    };
    [Parameter] public List<NestedModel> Items { get; set; }
    [Parameter] public int Depth { get; set; }
}