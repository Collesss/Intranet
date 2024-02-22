using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Intranet.Models
{
    public class UserViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [Display(Name = "ФИО")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Почта")]
        public string Email { get; set; } = string.Empty;
        
        public PhoneUserViewModel[] Phones { get; set; } = Array.Empty<PhoneUserViewModel>();
    }
}
