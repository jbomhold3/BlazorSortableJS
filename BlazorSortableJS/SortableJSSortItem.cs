using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorSortableJS
{
    public class SortableJSSortItem<T>
    {
        public int DataId { get; set; }
        public T Data { get; set; }
}
}
