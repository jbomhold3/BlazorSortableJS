using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorSortableJS
{
    public static class SortableJSEventHandler
    {
        public static EventHandler<SortableJSEvent> OnChooseEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnUnchooseEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnStartEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnEndEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnAddEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnUpdateEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnSortEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnRemoveEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnFilterEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnMoveEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnCloneEvent { get; set; }
        public static EventHandler<SortableJSEvent> OnChangeEvent { get; set; }

        [JSInvokable]
        public static Task OnChoose(SortableJSEvent data)
        {
            OnChooseEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnUnchoose(SortableJSEvent data)
        {
            OnUnchooseEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnStart(SortableJSEvent data)
        {
            OnStartEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnEnd(SortableJSEvent data)
        {
            OnEndEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnAdd(SortableJSEvent data)
        {
            OnAddEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnUpdate(SortableJSEvent data)
        {
            OnUpdateEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnSort(SortableJSEvent data)
        {
            OnSortEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnRemove(SortableJSEvent data)
        {
            OnRemoveEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnFilter(SortableJSEvent data)
        {
            OnFilterEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnMove(SortableJSEvent data)
        {
            OnMoveEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnClone(SortableJSEvent data)
        {
            OnCloneEvent?.Invoke(data.RefId, data);
            return default;
        }

        [JSInvokable]
        public static Task OnChange(SortableJSEvent data)
        {
            OnChangeEvent?.Invoke(data.RefId, data);
            return default;
        }
    }
}
