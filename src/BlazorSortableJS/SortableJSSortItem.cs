using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorSortableJS
{
    public class SortableJSSortItem<T>
    {
        public string DataId { get; set; }
        public bool Show { get; set; } = true;
        public T Data { get; set; }
    }
}
