using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Collections.Generic;

namespace BlazorSortableJS.Components
{
    public class DynamicElement : ComponentBase
    {
        /// <summary>
        /// Gets or sets the name of the element to render.
        /// </summary>
        [Parameter] public string TagName { get; set; }

        [Parameter] public RenderFragment ChildContent { get; set; }

        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> MyParams { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);
            builder.OpenElement(0, TagName);
            builder.AddMultipleAttributes(1, MyParams);
            builder.AddContent(3, ChildContent);
            builder.CloseElement();
        }
    }
}