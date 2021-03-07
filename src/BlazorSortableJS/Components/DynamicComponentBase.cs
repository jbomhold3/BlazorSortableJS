using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlazorSortableJS.Components
{
    public abstract class DynamicComponentBase : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> UnknownParameters { get; set; }
    }
}