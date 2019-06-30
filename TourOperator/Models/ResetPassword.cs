using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TourOperator.Models
{
    public class ResetPassword
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "New Password Required")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword" , ErrorMessage = "New Password and Reset Password do not match")]
        [Required(ErrorMessage = "Re-Enter the password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Reset Code is required")]
        public string ResetCode { get; set; }
    }
}