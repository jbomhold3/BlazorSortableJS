using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorSortableJS
{
    public class SortableJsOptions
    {

        public string group { get; set; }
        public bool? sort { get; set; }
        public bool? disabled { get; set; } 
        public string handle { get; set; }
        public string draggable { get; set; }
        public int? swapThreshold { get; set; }  
        public bool? invertSwap { get; set; }  
        public int? invertedSwapThreshold { get; set; } 
        public int? delay { get; set; } 
        public bool? delayOnTouchOnly { get; set; } 
        public int? touchStartThreshold { get; set; } 
        public bool? removeCloneOnHide { get; set; } 
        public string direction { get; set; }
        public string ghostClass { get; set; } 
        public string chosenClass { get; set; } 
        public string dragClass { get; set; } 
        public string filter { get; set; }
        public bool? preventOnFilter { get; set; }

        //  store: null,  // @see Store
        public double? animation { get; set; }
        public string easing { get; set; }

        public string dataIdAttr { get; set; } 
        public bool? forceFallback { get; set; } 

        public string fallbackClass { get; set; }
        public bool? fallbackOnBody { get; set; } 
        public int? fallbackTolerance { get; set; } 

        public bool? dragoverBubble { get; set; } 
        
        public int? emptyInsertThreshold { get; set; } 


        //setData: function(/** DataTransfer */dataTransfer, /** HTMLElement*/dragEl) {

        //dataTransfer.setData('Text', dragEl.textContent); // `dataTransfer` object of HTML5 DragEvent
        //},

        // Element is chosen
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
                if(pr.PropertyType == typeof(Action<SortableJSEvent>))
                {

                }
                else if (val is string && string.IsNullOrWhiteSpace(val.ToString()))
                {
                }
                else if (val == null)
                {
                }
                else
                {
                    returnClass.Add(pr.Name, val);
                }
            }
            return returnClass;
        }
    }

}