using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json.Serialization;
namespace BlazorSortableJS
{
    public class SortableJS<T> : IAsyncDisposable
    {
        private readonly IJSRuntime _jSRuntime;
        private SortableJsOptions _opt { get; set; }
        public EventHandler OnDataSet { get; set; }
        private Guid _refId { get; set; }

        public Guid RefId
        {
            get { return _refId;}
        }
        private string _elId { get; set; }
        private List<SortableJSSortItem<T>> _list { get; set; } = new List<SortableJSSortItem<T>>();

        public List<SortableJSSortItem<T>> GetRaw()
        {

            return _list.Where(q => q.Show == true).ToList();
        }

        public SortableJS(IJSRuntime jsRuntime)
        {
            _refId = Guid.NewGuid();
            _jSRuntime = jsRuntime;
        }
        public async Task Create(string elId, SortableJsOptions opt)
        {
            _opt = opt;
            _elId = elId;
            await _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.Create", _refId, elId, opt.RemoveNulls());

            //Register to all Events
            SortableJSEventHandler.OnChooseEvent += OnChoose;
            SortableJSEventHandler.OnUnchooseEvent += OnUnchoose;
            SortableJSEventHandler.OnStartEvent += OnStart;
            SortableJSEventHandler.OnEndEvent += OnEnd;
            SortableJSEventHandler.OnAddEvent += OnAdd;
            SortableJSEventHandler.OnUnchooseEvent += OnUpdate;
            SortableJSEventHandler.OnSortEvent += OnSort;
            SortableJSEventHandler.OnRemoveEvent += OnRemove;
            SortableJSEventHandler.OnFilterEvent += OnFilter;
            SortableJSEventHandler.OnMoveEvent += OnMove;
            SortableJSEventHandler.OnCloneEvent += OnClone;
            SortableJSEventHandler.OnChangeEvent += OnChange;
        }
        public void SetData(List<T> list)
        {
            foreach (var item in list)
            {
                _list.Add(new SortableJSSortItem<T>() { Data = item, DataId = Guid.NewGuid().ToString() });
            }
            OnDataSet?.Invoke(new object(), new EventArgs());
        }
        public async Task<List<T>> GetData()
        {

            var indexs = await ToArray() ;
            //var order = indexs.ToList();
            //var temp = order.Select(i => _list[i]).ToList();
            var result = new List<T>();
            foreach (var index in indexs)
            {
                result.Add(_list.First(q => q.DataId == index).Data);
            }
            return result;
        }

        public Task Sort(string[] order)
        {
        if(_refId == default) throw new InvalidOperationException("Create Function must be called first");
            return _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.Sort", _refId, order);
        }
        public Task<string[]> ToArray()
        {
            if (_refId == default) throw new InvalidOperationException("Create Function must be called first");
            return _jSRuntime?.InvokeAsync<string[]>("BlazorSortableJS.ToArray", _refId);
        }
        public Task Distroy()
        {
            if (_refId == default) throw new InvalidOperationException("Create Function must be called first");
            return _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.Destroy", _refId);
        }
        public void Add(string dataId, string data)
        {
            var newItem = JsonSerializer.Parse<T>(data);
            _list.Add(new SortableJSSortItem<T>() { DataId = dataId, Data = newItem, Show = false});
        }
        public void Dispose()
        {
            SortableJSEventHandler.OnChooseEvent -= OnChoose;
            SortableJSEventHandler.OnUnchooseEvent -= OnUnchoose;
            SortableJSEventHandler.OnStartEvent -= OnStart;
            SortableJSEventHandler.OnEndEvent -= OnEnd;
            SortableJSEventHandler.OnAddEvent -= OnAdd;
            SortableJSEventHandler.OnUnchooseEvent -= OnUpdate;
            SortableJSEventHandler.OnSortEvent -= OnSort;
            SortableJSEventHandler.OnRemoveEvent -= OnRemove;
            SortableJSEventHandler.OnFilterEvent -= OnFilter;
            SortableJSEventHandler.OnMoveEvent -= OnMove;
            SortableJSEventHandler.OnCloneEvent -= OnClone;
            SortableJSEventHandler.OnChangeEvent -= OnChange;
        }

        public void OnChoose(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) != _refId) return;
            _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.SetChoiceItem", _refId, _list.First(q => q.DataId == data.DataId).Data);
            _opt.OnChoose?.Invoke(data);
            Console.WriteLine(data.DataId);
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
            var json = JsonSerializer.ToString(data.Data);
            Add(data.DataId,json);
         
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

        public async void OnRemove(object sender, SortableJSEvent data)
        {
            // DO NOT remove items from _list here. SortableJS will remove the index
            // Getdata will only return the indexs SortableJS provides it.
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

        public async ValueTask DisposeAsync()
        {
            await Distroy();
            Dispose();
        }
    }
}
