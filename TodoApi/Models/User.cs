using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string Address { get; set; }
    }
}
