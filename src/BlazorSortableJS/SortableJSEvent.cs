using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
namespace BlazorSortableJS
{
    public class SortableJSEvent
    {
        public Guid RefId { get; set; }
        public string DataId { get; set; }
        public object Data { get;set;}
        public object Item { get; set; }
        public object To { get; set; }
        public object From { get; set; }
        public int OldIndex { get; set; }
        public int newIndex { get; set; }
        public int OldDraggableIndex { get; set; }
        public int NewDraggableIndex { get; set; }
        public object Clone { get; set; }
        public object PullMode { get; set; }
        public object Dragged { get; set; }
        public DOMRect DraggedRect { get; set; }
        public object Related { get; set; }
        public DOMRect RelatedRect { get; set; }
        public object WillInsertAfter { get; set; }
        public int clientY { get; set; }
        public int clientX { get; set; }
    }
}
