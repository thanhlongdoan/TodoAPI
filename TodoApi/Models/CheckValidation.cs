using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class CheckValidation : ValidationAttribute
    {

    }
    public class CheckBirthday : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
                        ValidationContext validationContext)
        {
            var model = (AddUserViewModel)validationContext.ObjectInstance;
            var url = "https://localhost:44359/api/user/CheckBirthday?birthday=" + model.Birthday;
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Ngày sinh lớn hơn ngày hiện tại");
        }
    }
    public class CheckEmail : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
                        ValidationContext validationContext)
        {
            var model = (AddUserViewModel)validationContext.ObjectInstance;
            var url = "https://localhost:44359/api/user/CheckEmail?email=" + model.Email;
            HttpClient client = new HttpClient();
            HttpResponseMessage responseMessage = client.GetAsync(url).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Email đã tồn tại");
        }
    }
}
