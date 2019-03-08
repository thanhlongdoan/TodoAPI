using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class PageModel
    {
        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public IEnumerable<TodoItem> items { get; set; }
    }
}
