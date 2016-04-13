using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ImmigrantBlog.WEB.ModelViews
{
    public class RegisterModelView
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "*Некорректный адрес")]
        [System.Web.Mvc.Remote("CheckEmail", "Account", ErrorMessage = "*Email уже используется")]
        public string Email { get; set; }
        [Required]
        [System.Web.Mvc.Remote("CheckLogin", "Account", ErrorMessage = "*Login уже занят")]
        public string Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "*Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}