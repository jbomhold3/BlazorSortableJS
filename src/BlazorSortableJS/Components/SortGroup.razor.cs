using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorSortableJS.Components
{
    public partial class SortGroup<TItem> : ComponentBase
    {
        [Inject] private IJSRuntime jSRuntime { get; set; }
        [Parameter] public string Id { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public string TemplateClass { get; set; }
        [Parameter] public string GroupName { get; set; } = Guid.NewGuid().ToString();
        [Parameter] public string Handle { get; set; }
        [Parameter] public string Style { get; set; }
        [Parameter] public bool IsDiv { get; set; }
        [Parameter] public RenderFragment<SortableJSSortItem<TItem>> Template { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> TemplateOnClick { get; set; }

        [Parameter] public List<TItem> Items { get; set; }
           
        [Parameter] public EventCallback<SortableEvent<TItem>> OnChoose { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnUnchoose { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnStart { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnEnd { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnAdd { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnUpdate { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnSort { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnRemove { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnFilter { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnMove { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnClone { get; set; }
        [Parameter] public EventCallback<SortableEvent<TItem>> OnChange { get; set; }
        public SortableJS<TItem> Sortable { get; set; }
        private bool Render;
        private bool ForceReDraw;
        private bool ForceDelay;

        public async Task<List<TItem>> GetOrderListAsync(string orderColumn)
        {
            var items = await Sortable.GetDataAsync();
            if (!string.IsNullOrEmpty(orderColumn))
            {
                for (var i = 0; i < items.Count; ++i)
                {
                    var type = typeof(TItem);
                    PropertyInfo pi = type.GetProperty(orderColumn);
                    pi.SetValue(items[i], i);
                }
            }
            return items;
        }

        private string TemplateTag
        {
            get
            {
                return (IsDiv) ? "div" : "li";
            }
        }
        private string Tag
        {
            get
            {
                return (IsDiv) ? "div" : "ul";
            }
        }
        
        protected override Task OnInitializedAsync()
        {
            if (Id == null)
                Id = Guid.NewGuid().ToString();
            Sortable = new SortableJS<TItem>(jSRuntime);
            Sortable.SetData(Items);
            return base.OnInitializedAsync();
        }
        public async void OnChooseEvent(SortableJSEvent e)
        {
            await OnChoose.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnUnchooseEvent(SortableJSEvent e)
        {
            await OnUnchoose.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnStartEvent(SortableJSEvent e)
        {
            await OnStart.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnEndEvent(SortableJSEvent e)
        {
            await OnEnd.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnAddEvent(SortableJSEvent e)
        {
            await OnAdd.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnUpdateEvent(SortableJSEvent e)
        {
            await OnUpdate.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnSortEvent(SortableJSEvent e)
        {
            await OnSort.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnRemoveEvent(SortableJSEvent e)
        {
            await OnRemove.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnFilterEvent(SortableJSEvent e)
        {
            await OnFilter.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnMoveEvent(SortableJSEvent e)
        {
            await OnMove.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnCloneEvent(SortableJSEvent e)
        {
            await OnClone.InvokeAsync(new SortableEvent<TItem>(this,e));
        }
        public async void OnChangeEvent(SortableJSEvent e)
        {
            await OnChange.InvokeAsync(new SortableEvent<TItem>(this,e));
        }

        public async Task UpdateAsync()
        {
            await Sortable.ClearAsync();
            Sortable = new SortableJS<TItem>(jSRuntime);
            Sortable.SetData(Items);
            ForceReDraw = true;
            await InvokeAsync(StateHasChanged);
        }
        private async Task Start()
        {
            await Sortable.CreateAsync(Id, new SortableJsOptions
            {
                Group = GroupName,
                Handle = Handle,
                Animation = 100,
                OnClone = OnCloneEvent,
                OnAdd = OnAddEvent,
                OnChange = OnChangeEvent,
                OnChoose = OnChooseEvent,
                OnEnd = OnEndEvent,
                OnFilter = OnFilterEvent,
                OnMove = OnMoveEvent,
                OnRemove = OnRemoveEvent,
                OnSort = OnSortEvent,
                OnStart = OnStartEvent,
                OnUnchoose = OnUnchooseEvent,
                OnUpdate = OnUpdateEvent,
            });
        }
        protected override async Task OnAfterRenderAsync(bool firstrun)
        {
            if (ForceReDraw)
            {
                ForceDelay = true;
                ForceReDraw = false;
                await InvokeAsync(StateHasChanged);
            }
            // Fix for client side. Ensures Element is there before continuing.
            else if (ForceDelay)
            {
                ForceDelay = false;
                Render = true;
                await InvokeAsync(StateHasChanged);
            }
            if (firstrun || Render)
            {
                Render = false;
                await Start();
            }
        }
    }

    public class SortableEvent<T>
    {
        public SortGroup<T> Sender { get; set; }
        public T Data { get; set; }
        public SortableJSEvent Event { get; set; }
        public SortableEvent(SortGroup<T> sender, SortableJSEvent e)
        {
            Sender = sender;
            try
            {
                Data = JsonSerializer.Deserialize<T>(e.Data.ToString(),
                    new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
            }
            catch{}

            Event = e;
        }
    }
}
