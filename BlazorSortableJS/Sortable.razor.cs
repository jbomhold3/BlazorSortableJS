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
        private List<TItem>? _lastItemsReference;
        [Parameter]
        public RenderFragment<TItem> Template { get; set; } = null!;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public Func<Task>? OnDataChanged { get; set; }

        /// <summary>
        /// Callback fired when an item is dropped into this sortable (either from another list or reordered within)
        /// </summary>
        [Parameter]
        public Func<SortableDroppedEventArgs<TItem>, Task>? OnItemDropped { get; set; }

        /// <summary>
        /// Optional identifier for this sortable, useful for identifying which list received a drop
        /// </summary>
        [Parameter]
        public string Id { get; set; } = string.Empty;

        [Parameter]
        public string Class { get; set; } = string.Empty;
        [Parameter]
        public object? Options { get; set; } 

        internal ElementReference _dropZoneContainer;
        private bool _shouldRender = true;
        private bool _hasPreRendered;

        protected override void OnParametersSet()
        {
            // Check if the list REFERENCE changed (not just contents)
            // This is critical because JS holds a reference to the list
            if (!ReferenceEquals(_lastItemsReference, Items))
            {
                _lastItemsReference = Items;
                _shouldRender = true; // Force re-render to re-initialize JS with new list
            }

            if (!_lastItems.SequenceEqual(Items))
            {
                _lastItems = Items.Select(x => x).ToList();
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
                    // Destroy existing sortable before re-initializing (important for list reference changes)
                    if (!firstRender)
                    {
                        await ParentSortable.DestroySortableAsync(this);
                    }
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

            // Fire OnItemDropped for same-list reorder
            if (OnItemDropped is not null)
            {
                var args = new SortableDroppedEventArgs<TItem>
                {
                    Item = item,
                    TargetSortableId = Id,
                    SourceSortableId = Id, // Same list
                    NewIndex = newIndex,
                    OldIndex = oldIndex,
                    ItemBefore = newIndex > 0 ? Items[newIndex - 1] : default,
                    ItemAfter = newIndex < Items.Count - 1 ? Items[newIndex + 1] : default
                };
                await OnItemDropped.Invoke(args);
                // When OnItemDropped is used, consumer handles updates - skip refresh
                return;
            }

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
            // Note: RemoveItem is called on source list during cross-list moves
            // OnItemDropped will be called on the target list's AddItem
            // Only trigger refresh/callbacks if not using OnItemDropped pattern
            if (OnItemDropped is not null)
            {
                // Consumer is handling via OnItemDropped on target - skip refresh
                return;
            }

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
        public async Task AddItem(int index, TItem item, string? sourceSortableId = null, int oldIndex = -1)
        {
            Items.Insert(index, item);

            // Fire OnItemDropped for cross-list drop
            if (OnItemDropped is not null)
            {
                var args = new SortableDroppedEventArgs<TItem>
                {
                    Item = item,
                    TargetSortableId = Id,
                    SourceSortableId = sourceSortableId,
                    NewIndex = index,
                    OldIndex = oldIndex,
                    ItemBefore = index > 0 ? Items[index - 1] : default,
                    ItemAfter = index < Items.Count - 1 ? Items[index + 1] : default
                };
                await OnItemDropped.Invoke(args);
                // When OnItemDropped is used, consumer handles updates - skip refresh
                return;
            }

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

        /// <summary>
        /// Moves an item to a specific index, removing it from its current position if found.
        /// Useful for syncing state from external updates (e.g., dispatcher messages).
        /// </summary>
        /// <param name="item">The item to move</param>
        /// <param name="newIndex">The target index</param>
        /// <returns>True if the item was found and moved, false if inserted as new</returns>
        public bool MoveOrInsertItem(TItem item, int newIndex)
        {
            var existingIndex = Items.IndexOf(item);
            if (existingIndex >= 0)
            {
                if (existingIndex == newIndex) return true; // Already in position
                Items.RemoveAt(existingIndex);
                if (newIndex > existingIndex) newIndex--; // Adjust for removal
            }

            if (newIndex < 0) newIndex = 0;
            if (newIndex > Items.Count) newIndex = Items.Count;

            Items.Insert(newIndex, item);
            return existingIndex >= 0;
        }

        /// <summary>
        /// Removes an item from the list if found.
        /// </summary>
        /// <param name="item">The item to remove</param>
        /// <returns>True if the item was found and removed</returns>
        public bool RemoveItemIfExists(TItem item)
        {
            return Items.Remove(item);
        }

        /// <summary>
        /// Syncs the items list to match the provided order.
        /// Items not in the new order are removed, new items are added.
        /// </summary>
        /// <param name="orderedItems">The new ordered list of items</param>
        public void SyncItems(List<TItem> orderedItems)
        {
            Items.Clear();
            Items.AddRange(orderedItems);
        }
    }
}