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

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Ngày sinh bắt buộc nhập")]
        public DateTime Birthday { get; set; }

        [Range(1, 3, ErrorMessage = "Lựa chọn không đúng")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Email bắt buộc nhập")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại phải nhập")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{10})\)?$", ErrorMessage = "Số điện thoại sai định dạng")]
        public string NumberPhone { get; set; }

        public string Address { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; }
    }

    public class GetUserViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public TimeSpan Birthday { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string Address { get; set; }

        public ICollection<TodoItem> TodoItems { get; set; }
    }
}
