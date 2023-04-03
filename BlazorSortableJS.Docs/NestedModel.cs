namespace BlazorSortableJS.Docs
{
    public class NestedModel
    {
        public string Data { get; set; } = string.Empty;
        public List<NestedModel> Children { get; set; } = new List<NestedModel>();
    }
}
