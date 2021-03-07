using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace BlazorSortableJS.Components
{
    public partial class SortableItem<TItem> : DynamicComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public string Style { get; set; }
        [Parameter] public bool IsDiv { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public SortableJSSortItem<TItem> Item { get; set; }
        [CascadingParameter] public SortableJS<TItem> Sortable { get; set; }


        string Tag
        {
            get
            {
                return (IsDiv) ? "div" : "li";
            }
        }

        protected override Task OnAfterRenderAsync(bool firstrun)
        {
            return base.OnAfterRenderAsync(false);
        }
    }
}
