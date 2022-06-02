using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class ProfileFormViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "PESEL")]
        public string PESEL { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

    }
}
