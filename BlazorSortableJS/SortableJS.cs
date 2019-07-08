using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
namespace BlazorSortableJS
{
    public class SortableJS : IDisposable
    {
        
        private readonly IJSRuntime _jSRuntime;
        private SortableJsOptions _opt { get; set; }
        private Guid _refId { get; set; }

        public SortableJS(IJSRuntime jsRuntime)
        {
            _jSRuntime = jsRuntime;
        }
        public async Task Set(string elId, SortableJsOptions opt)
        {
            var refId = Guid.NewGuid();
            _refId = refId;
            _opt = opt;

            await _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.SortableJs", refId, elId, opt.RemoveNulls());

            //Register to all Events
            SortableJSEventHandler.OnChooseEvent += OnChoose;
            SortableJSEventHandler.OnChooseEvent += OnUnchoose;
            SortableJSEventHandler.OnChooseEvent += OnStart;
            SortableJSEventHandler.OnChooseEvent += OnEnd;
            SortableJSEventHandler.OnChooseEvent += OnAdd;
            SortableJSEventHandler.OnChooseEvent += OnUpdate;
            SortableJSEventHandler.OnChooseEvent += OnSort;
            SortableJSEventHandler.OnChooseEvent += OnRemove;
            SortableJSEventHandler.OnChooseEvent += OnFilter;
            SortableJSEventHandler.OnChooseEvent += OnMove;
            SortableJSEventHandler.OnChooseEvent += OnClone;
            SortableJSEventHandler.OnChooseEvent += OnChange;


        }

        public void Dispose()
        {
            SortableJSEventHandler.OnChooseEvent -= OnChoose;
            SortableJSEventHandler.OnChooseEvent -= OnUnchoose;
            SortableJSEventHandler.OnChooseEvent -= OnStart;
            SortableJSEventHandler.OnChooseEvent -= OnEnd;
            SortableJSEventHandler.OnChooseEvent -= OnAdd;
            SortableJSEventHandler.OnChooseEvent -= OnUpdate;
            SortableJSEventHandler.OnChooseEvent -= OnSort;
            SortableJSEventHandler.OnChooseEvent -= OnRemove;
            SortableJSEventHandler.OnChooseEvent -= OnFilter;
            SortableJSEventHandler.OnChooseEvent -= OnMove;
            SortableJSEventHandler.OnChooseEvent -= OnClone;
            SortableJSEventHandler.OnChooseEvent -= OnChange;
        }

        public void OnChoose(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnChoose?.Invoke(data);
        }
        public void OnUnchoose(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnUnchoose?.Invoke(data);
        }
        public void OnStart(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnStart?.Invoke(data);
        }
        public void OnEnd(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnEnd?.Invoke(data);
        }
        public void OnAdd(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnAdd?.Invoke(data);
        }
        public void OnUpdate(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnUpdate?.Invoke(data);
        }
        public void OnSort(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnSort?.Invoke(data);
        }
        public void OnRemove(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnRemove?.Invoke(data);
        }
        public void OnFilter(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnFilter?.Invoke(data);
        }
        public void OnMove(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnMove?.Invoke(data);
        }
        public void OnClone(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnClone?.Invoke(data);
        }
        public void OnChange(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _opt.OnChange?.Invoke(data);
        }
    }
}
