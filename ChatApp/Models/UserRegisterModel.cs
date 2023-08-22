using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ChatApp.Models
{
    public class UserRegisterModel
    {
        [Required(ErrorMessage = "User's Name is required")]
        [DataType(DataType.Text)]
        [MinLength(3)]
        [MaxLength(30)]
        [Display(Name = "User's Name")]
        public string name { get; set; }

        [RegularExpression(@"\A(?:[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?\.)+[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z3333333330-9])?)\Z",
                            ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Id ")]
        [Required(ErrorMessage = "Email Id is required")]
        public string email_Id { get; set; }

        [Required(ErrorMessage = "Set the password")]
        [Display(Name = "Password")]
        [MinLength(4)]
        [MaxLength(1024)]
        public string password { get; set; }
    }
}