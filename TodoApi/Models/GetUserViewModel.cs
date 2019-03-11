using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class GetUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public long Birthday { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string Address { get; set; }
    }
}
