using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorSortableJS
{
    public partial class Sortable<TItem> : IAsyncDisposable
    {
        [CascadingParameter]
        public SortableWrapper? ParentSortable { get; set; }

        [Parameter]
        public List<TItem> Items { get; set; } = new List<TItem> { };

        [Parameter]
        public RenderFragment<TItem> Template { get; set; } = null!;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string Class { get; set; } = string.Empty;
        [Parameter]
        public object? Options { get; set; } 

        internal ElementReference _dropZoneContainer;
        private bool _shouldRender = true;
        private bool _hasPreRendered;
        protected override void OnInitialized()
        {
            if(ParentSortable != null)
                ParentSortable.OnRefresh += ParentSortable_OnRefresh;
        }

        private async Task ParentSortable_OnRefresh()
        {
            _shouldRender = true;
            if (ParentSortable != null)
                await ParentSortable.DestroySortableAsync(this);
            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _hasPreRendered = true;
            }
            if (_hasPreRendered && _shouldRender)
            {
                if (ParentSortable != null)
                {
                    await Task.Delay(100);
                    await ParentSortable.NotifyDropZoneRendered(this);
                }
                _shouldRender = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (ParentSortable != null && _hasPreRendered)
            {
                try
                {
                    await JSRuntime.InvokeVoidAsync("destroySortable", _dropZoneContainer);
                }
                catch { }
                if(ParentSortable.OnRefresh != null)
                    ParentSortable.OnRefresh -= ParentSortable_OnRefresh;
            }
        }

        protected override bool ShouldRender()
        {
            return _shouldRender;
        }

        [JSInvokable]
        public void UpdateItemOrder(int oldIndex, int newIndex)
        {
            var item = Items[oldIndex];
            Items.RemoveAt(oldIndex);
            Items.Insert(newIndex, item);
            if (ParentSortable != null && ParentSortable.OnDataChanged.HasDelegate)
                _ = ParentSortable.OnDataChanged.InvokeAsync();
        }

        [JSInvokable]
        public void RemoveItem(int index)
        {
            Items.RemoveAt(index);
            if (ParentSortable != null && ParentSortable.OnDataChanged.HasDelegate)
                _ = ParentSortable.OnDataChanged.InvokeAsync();
        }

        [JSInvokable]
        public void AddItem(int index, TItem item)
        {
            Items.Insert(index, item);
            if (ParentSortable != null && ParentSortable.OnDataChanged.HasDelegate)
                _ = ParentSortable.OnDataChanged.InvokeAsync();
        }

    }
}