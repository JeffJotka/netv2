using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace WebApplication2.Models
{
    public class Doctor
    {
        [Key]
        [Required]
        public long Id { get; set; }
       
        [StringLength(100, ErrorMessage = "Maximum Length is {1}")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public IList<Appointment> Appointments { get; set; }
        [Required]
        public long SpecializationId { get; set; }
        [ForeignKey(name: "SpecializationId")]
        [Display(Name = "Specialization")]
        public Specialization Specialization { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        //public string DoctorName
        //{
        //    get { return $"{FirstName} {LastName}"; }
        //}
    }
}
