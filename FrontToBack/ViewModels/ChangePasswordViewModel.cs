using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Username { get; set; }

        [Required,DataType(DataType.Password)]
        public string OldPasswor { get; set; }

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
