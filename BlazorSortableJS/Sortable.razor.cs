using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorSortableJS
{
    public partial class Sortable<TItem> : IAsyncDisposable
    {
        [CascadingParameter]
        public SortableWrapper? ParentSortable { get; set; }

        [Parameter]
        public List<TItem> Items { get; set; } = new List<TItem> { };
        private List<KeyedItem<TItem>> _keyedItems = new();
        private List<TItem> _lastItems = new();
        [Parameter]
        public RenderFragment<TItem> Template { get; set; } = null!;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public Func<Task>? OnDataChanged { get; set; }

        [Parameter]
        public string Class { get; set; } = string.Empty;
        [Parameter]
        public object? Options { get; set; } 

        internal ElementReference _dropZoneContainer;
        private bool _shouldRender = true;
        private bool _hasPreRendered;

        protected override void OnParametersSet()
        {
            if (!_lastItems.SequenceEqual(Items))
            {
                _lastItems = Items.Select(x => x).ToList() ;
                _keyedItems = Items.Select((item, index) => new KeyedItem<TItem> { Key = Guid.NewGuid().ToString(), Item = item }).ToList();

            }
        }

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

            if (!_lastItems.SequenceEqual(Items))
            {
                _lastItems = Items.Select(x => x).ToList();
                _keyedItems = Items.Select((item, index) => new KeyedItem<TItem> { Key = Guid.NewGuid().ToString(), Item = item }).ToList();

            }
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
        public async Task UpdateItemOrder(int oldIndex, int newIndex)
        {
            var item = Items[oldIndex];
            Items.RemoveAt(oldIndex);
            Items.Insert(newIndex, item);
            if (ParentSortable != null)
            {
                if (ParentSortable.OnDataChanged.HasDelegate)
                    await ParentSortable.OnDataChanged.InvokeAsync();
                await ParentSortable.RefreshAsync();
            }
            if(OnDataChanged is not null)
            {
                await OnDataChanged.Invoke();
            }
        }

        [JSInvokable]
        public async Task RemoveItem(int index)
        {
            Items.RemoveAt(index);
            if (ParentSortable != null)
            {
                if (ParentSortable.OnDataChanged.HasDelegate)
                    await ParentSortable.OnDataChanged.InvokeAsync();
                await ParentSortable.RefreshAsync();
            }
            if (OnDataChanged is not null)
            {
                await OnDataChanged.Invoke();
            }
        }

        [JSInvokable]
        public async Task AddItem(int index, TItem item)
        {
            Items.Insert(index, item );
            if (ParentSortable != null)
            {
                if (ParentSortable.OnDataChanged.HasDelegate)
                    await ParentSortable.OnDataChanged.InvokeAsync();
                await ParentSortable.RefreshAsync();
            }
            if (OnDataChanged is not null)
            {
                await OnDataChanged.Invoke();
            }
        }

    }
}