using System;
using System.Collections.Generic;

namespace BlazorSortableJS
{
    public class SortableJsOptions
    {
        public string Group { get; set; }
        public bool? Sort { get; set; }
        public bool? Disabled { get; set; }
        public string Handle { get; set; }
        public string Draggable { get; set; }
        public int? SwapThreshold { get; set; }
        public bool? InvertSwap { get; set; }
        public int? InvertedSwapThreshold { get; set; }
        public int? Delay { get; set; }
        public bool? DelayOnTouchOnly { get; set; }
        public int? TouchStartThreshold { get; set; }
        public bool? RemoveCloneOnHide { get; set; }
        public string Direction { get; set; }
        public string GhostClass { get; set; }
        public string ChosenClass { get; set; }
        public string DragClass { get; set; }
        public string Filter { get; set; }
        public bool? PreventOnFilter { get; set; }

        public double? Animation { get; set; }
        public string Easing { get; set; }

        public string DataIdAttr { get; set; }
        public bool? ForceFallback { get; set; }

        public string FallbackClass { get; set; }
        public bool? FallbackOnBody { get; set; }
        public int? FallbackTolerance { get; set; }

        public bool? DragoverBubble { get; set; }

        public int? EmptyInsertThreshold { get; set; }

        public Action<SortableJSEvent> OnChoose { get; set; }
        public Action<SortableJSEvent> OnUnchoose { get; set; }
        public Action<SortableJSEvent> OnStart { get; set; }
        public Action<SortableJSEvent> OnEnd { get; set; }
        public Action<SortableJSEvent> OnAdd { get; set; }
        public Action<SortableJSEvent> OnUpdate { get; set; }
        public Action<SortableJSEvent> OnSort { get; set; }
        public Action<SortableJSEvent> OnRemove { get; set; }
        public Action<SortableJSEvent> OnFilter { get; set; }
        public Action<SortableJSEvent> OnMove { get; set; }
        public Action<SortableJSEvent> OnClone { get; set; }
        public Action<SortableJSEvent> OnChange { get; set; }
    }

    public static class ReClasser
    {
        public static object RemoveNulls(this SortableJsOptions fixMe)
        {
            var t = fixMe.GetType();
            var returnClass = new Dictionary<string, object>();

            foreach (var pr in t.GetProperties())
            {
                var val = pr.GetValue(fixMe);
                if (pr.PropertyType != typeof(Action<SortableJSEvent>)
                    && val != null
                    && !string.IsNullOrWhiteSpace(val.ToString()))
                {
                    returnClass.Add(pr.Name, val);
                }
            }

            return returnClass;
        }
    }
}