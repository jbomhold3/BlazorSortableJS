# BlazorSortableJS
This is a very early project. However, it does function and is very much usable. The user experience will get better over.

[![NuGet Pre Release](https://img.shields.io/nuget/vpre/BlazorSortableJS.svg)](https://www.nuget.org/packages/BlazorSortableJS/)
- _host.cshtml
``` html 
<script src="https://cdn.jsdelivr.net/npm/sortablejs@latest/Sortable.min.js"></script>
<script src="_content/BlazorSortableJs/js/BlazorSortableJs.js"></script>
```
-  Usage
``` c#
@inject IJSRuntime jSRuntime;
```
``` html
Sortable List
<ul id="foo" class="list-group">
    @foreach (var item in rawList)
    {
        <li class="list-group-item" data-id="@item.DataId">@item.Data</li>
    }
</ul>
Resulting Data
<ul class="list-group">
    @foreach (var item in resultsList)
    {
        <li class="list-group-item">@item</li>
    }
</ul>
```    
``` c#
@code
{
    bool FirstRun = true;
    SortableJS<string> Sortable { get; set; }
    List<string> items { get; set; } = new List<string> { "test", "hello", "getoveryourself" };
    List<SortableJSSortItem<string>> rawList { get; set; } = new List<SortableJSSortItem<string>>();
    List<string> resultsList { get; set; } = new List<string>();

    protected override Task OnInitAsync()
    {
        resultsList = items;
        return base.OnInitAsync();
    }

    protected async override Task OnAfterRenderAsync()
    {
        if (FirstRun)
        {
            Sortable = new SortableJS<string>(jSRuntime);
            Sortable.SetData(items);
            rawList = Sortable.GetRaw();

            await Sortable.Create("foo", new SortableJsOptions
            {
                group = "foo",
                animation = 100,
                OnSort = async (e) => { await Sortable.GetData(); StateHasChanged()}
            });
            FirstRun = false;
            StateHasChanged();
        }
    }
}
```

## Change Log
https://github.com/jbomhold3/BlazorSortableJS/releases
