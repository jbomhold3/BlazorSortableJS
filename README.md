# BlazorSortableJS

BlazorSortableJS is a Blazor Wrapper over JavaScript [Sortable](https://github.com/SortableJS/Sortable) lib.

>Sortable is a JavaScript library for reorderable drag-and-drop lists.

This is a very early project. However, it does function and is very much usable. The user experience will get better over.

[![NuGet Pre Release](https://img.shields.io/nuget/vpre/BlazorSortableJS.svg)](https://www.nuget.org/packages/BlazorSortableJS/)
- _host.cshtml
``` html 
<script src="https://cdn.jsdelivr.net/npm/sortablejs@latest/Sortable.min.js"></script>
<script src="_content/BlazorSortableJs/js/BlazorSortableJs.js"></script>
```
-  Usage
``` html
Sortable List
<SortableGroup Class="list-group" TItem="string" @ref="MyGroup">
    @foreach (var item in @MyGroup.Sortable.GetRaw())
    {
        <SortableItem Class="list-group-item" Item="item" TItem="string">@item.Data</SortableItem>
    }
</SortableGroup>
```    
``` c#
@code
{
    SortableGroup<string> MyGroup;
    List<string> items { get; set; } = new List<string> { "T1", "T2", "T3" };
    List<string> resultsList { get; set; } = new List<string>();

    protected override Task OnInitializedAsync()
    {
        resultsList = items;
        return base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            MyGroup.Sortable.SetData(items);
            await MyGroup.Sortable.CreateAsync(MyGroup.Id, new SortableJsOptions
            {
                Group = "test",
                Animation = 100,
            });         

            StateHasChanged();
        }
    }
}
```

## Change Log
https://github.com/jbomhold3/BlazorSortableJS/releases
