using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Linq;
using System.Text.Json;
using System.Reflection;

namespace BlazorSortableJS
{
    public class SortableJS<T> : IAsyncDisposable
    {
        public Guid RefId { get; private set; } = Guid.NewGuid();
        public EventHandler OnDataSet { get; set; }

        private readonly IJSRuntime _jSRuntime;
        private readonly List<SortableJSSortItem<T>> _list = new List<SortableJSSortItem<T>>();

        private SortableJsOptions _opt;
        private string _elId;

        public SortableJS(IJSRuntime jsRuntime)
        {
            _jSRuntime = jsRuntime;
        }

        public List<SortableJSSortItem<T>> GetRaw()
        {
            return _list.Where(q => q.Show).ToList();
        }

        public async ValueTask<object> CreateAsync(string elId, SortableJsOptions opt)
        {
            _opt = opt;
            _elId = elId;

            await _jSRuntime.InvokeAsync<object>("BlazorSortableJS.Create", RefId, elId, opt.RemoveNulls());

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

            return default;
        }

        public void SetData(IEnumerable<T> list)
        {
            foreach (var item in list)
            {
                _list.Add(new SortableJSSortItem<T>()
                {
                    Data = item,
                    DataId = Guid.NewGuid().ToString()
                });
            }

            OnDataSet?.Invoke(new object(), new EventArgs());
        }

        public async Task ClearAsync()
        {
            await DestroyAsync();
            _list.Clear();
        }

        public void UpdateProperty(string id, string propertyName, object value)
        {
            var data = _list.Find(q => q.DataId == id);
            SetValue(data.Data, propertyName, value);
        }

        private void SetValue(object obj, string propertyName, object value)
        {
            Type type = typeof(T);
            PropertyInfo pi = type.GetProperty(propertyName);

            pi.SetValue(obj, Convert.ChangeType(value, pi.PropertyType), null);
        }

        public async Task AddItemAsync(T model)
        {
            await DestroyAsync();
            _list.Add(new SortableJSSortItem<T>() { Data = model, DataId = Guid.NewGuid().ToString() });
        }

        public async Task RemoveItemAsync(string id)
        {
            await DestroyAsync();
            var item = _list.FirstOrDefault(q => q.DataId == id);
            if (item != null)
            {
                _list.Remove(item);
            }
        }

        public async Task ResyncAsync()
        {
            await CreateAsync(_elId, _opt);
        }

        public void Update(string id, T model)
        {
            var data = _list.Find(q => q.DataId == id);

            if (data == null)
                return;

            data.Data = model;
        }

        public async Task<List<T>> GetDataAsync()
        {
            var indexs = await ToArrayAsync();
            //var order = indexs.ToList();
            //var temp = order.Select(i => _list[i]).ToList();
            var result = new List<T>();
            foreach (var index in indexs)
            {
                result.Add(_list.First(q => q.DataId == index).Data);
            }
            return result;
        }

        public ValueTask<object> SortAsync(string[] order)
        {
            if (RefId == default)
            {
                throw new InvalidOperationException("Create Function must be called first");
            }

            return _jSRuntime.InvokeAsync<object>("BlazorSortableJS.Sort", RefId, order);
        }

        public ValueTask<string[]> ToArrayAsync()
        {
            if (RefId == default)
            {
                throw new InvalidOperationException("Create Function must be called first");
            }

            return _jSRuntime.InvokeAsync<string[]>("BlazorSortableJS.ToArray", RefId);
        }

        public ValueTask<object> DestroyAsync()
        {
            if (RefId == default)
            {
                throw new InvalidOperationException("Create Function must be called first");
            }

            return _jSRuntime.InvokeAsync<object>("BlazorSortableJS.Destroy", RefId);
        }

        public void Add(string dataId, string data)
        {
            var newItem = JsonSerializer.Deserialize<T>(data);
            _list.Add(new SortableJSSortItem<T>() { DataId = dataId, Data = newItem, Show = false });
        }

        public void OnChoose(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _jSRuntime?.InvokeAsync<object>("BlazorSortableJS.SetChoiceItem", RefId, _list.First(q => q.DataId == data.DataId).Data);
                _opt.OnChoose?.Invoke(data);
            }
        }

        public void OnUnchoose(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnUnchoose?.Invoke(data);
            }
        }

        public void OnStart(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnStart?.Invoke(data);
            }
        }

        public void OnEnd(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnEnd?.Invoke(data);
            }
        }

        public void OnAdd(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                var json = JsonSerializer.Serialize(data.Data);
                Add(data.DataId, json);

                _opt.OnAdd?.Invoke(data);
            }
        }

        public void OnUpdate(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnUpdate?.Invoke(data);
            }
        }

        public void OnSort(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnSort?.Invoke(data);
            }
        }

        public void OnRemove(object sender, SortableJSEvent data)
        {
            // DO NOT remove items from _list here. SortableJS will remove the index
            // GetDataAsync will only return the indices SortableJS provides it.
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnRemove?.Invoke(data);
            }
        }

        public void OnFilter(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnFilter?.Invoke(data);
            }
        }

        public void OnMove(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnMove?.Invoke(data);
            }
        }

        public void OnClone(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnClone?.Invoke(data);
            }
        }

        public void OnChange(object sender, SortableJSEvent data)
        {
            if (Guid.Parse(sender.ToString()) == RefId)
            {
                _opt.OnChange?.Invoke(data);
            }
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

        public async ValueTask DisposeAsync()
        {
            await DestroyAsync();
            Dispose();
        }
    }
}
