using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.Models;

namespace WebApplication2.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        public List<Appointment> Appointments { get; set; }
        public Doctor Doctor { get; set; }   

        public Patient Patient { get; set; }   
    }
}
