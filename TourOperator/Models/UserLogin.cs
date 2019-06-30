using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TourOperator.Models
{
    public class UserLogin
    {
        public int Id { get; set; }

        [Display(Name = "Email or User Name")]
        [Required(AllowEmptyStrings = false , ErrorMessage = "Email or UserName is required")]
       
        public string EmailID { get; set; }

        

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is missing")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}