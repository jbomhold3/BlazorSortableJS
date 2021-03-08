# BlazorSortableJS

BlazorSortableJS is a Blazor Wrapper over JavaScript [Sortable](https://github.com/SortableJS/Sortable) lib.

>Sortable is a JavaScript library for reorderable drag-and-drop lists.

This is a very early project. However, it does function and is very much usable. The user experience will get better over time.

[![NuGet Pre Release](https://img.shields.io/nuget/vpre/BlazorSortableJS.svg)](https://www.nuget.org/packages/BlazorSortableJS/)
- _host.cshtml
``` html 
<script src="https://cdn.jsdelivr.net/npm/sortablejs@latest/Sortable.min.js"></script>
<script src="_content/BlazorSortableJs/js/BlazorSortableJs.js"></script>
```
-  Usage
``` html
Sortable List
<SortGroup Items="items" Id="@Id1" Class="list-group" TemplateClass="list-group-item" @ref="MyGroup" TItem="Items" OnSort="OnSort">
    <Template Context="item">
        @item.Data.Text
    </Template>
</SortGroup>
@resultsList
```    
``` c#
@code
{
    SortGroup<Items> MyGroup;
    List<Items> items { get; set; } = new List<Items>
    {
        new Items("T1"),
        new Items("T2"),
        new Items("T3"),
    };
    List<string> resultsList { get; set; } = new List<string>();
    
    public async Task OnSort(SortableEvent<Items> e)
    {
        ResultsList = await e.Sender.GetOrderListAsync("Order");
    }
    
     public class Items
    {
        public int Order { get; set; } = 0;
        public string Text { get; set; }

        public Items(string text)
        {
            Text = text;
        }
    }
}
```

## Change Log
https://github.com/jbomhold3/BlazorSortableJS/releases
