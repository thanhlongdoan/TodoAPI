using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class GetTodoViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }
        public TodoItem TodoItems { get; set; }
    }
}
