using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class DoctorModel
    {
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Doctor Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 digits")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Qualification is required")]
        [StringLength(50, ErrorMessage = "Qualification cannot be longer than 50 characters")]
        public string Qualification { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        [StringLength(50, ErrorMessage = "Specialization cannot be longer than 50 characters")]
        public string Specialization { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "UserID is required")]
        public int UserID { get; set; }
    }
}
