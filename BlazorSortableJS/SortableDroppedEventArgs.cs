namespace BlazorSortableJS;

public class SortableDroppedEventArgs<TItem>
{
    /// <summary>
    /// The item that was dropped
    /// </summary>
    public TItem Item { get; set; } = default!;

    /// <summary>
    /// The ID of the Sortable that received the drop
    /// </summary>
    public string TargetSortableId { get; set; } = string.Empty;

    /// <summary>
    /// The ID of the Sortable the item came from (null if same list reorder)
    /// </summary>
    public string? SourceSortableId { get; set; }

    /// <summary>
    /// The new index of the item in the target list
    /// </summary>
    public int NewIndex { get; set; }

    /// <summary>
    /// The old index of the item in the source list
    /// </summary>
    public int OldIndex { get; set; }

    /// <summary>
    /// The item before the dropped item in the list (null if dropped at start)
    /// </summary>
    public TItem? ItemBefore { get; set; }

    /// <summary>
    /// The item after the dropped item in the list (null if dropped at end)
    /// </summary>
    public TItem? ItemAfter { get; set; }

    /// <summary>
    /// True if this was a cross-list move, false if reorder within same list
    /// </summary>
    public bool IsCrossListMove => SourceSortableId != null && SourceSortableId != TargetSortableId;
}
