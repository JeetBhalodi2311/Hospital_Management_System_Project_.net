using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class StudentModel
    {
        public int StudentID { get; set; }

        [Required(ErrorMessage = "Enrollment number is required")]
        [Display(Name = "Enrollment No")]
        public string EnrollmentNo { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter valid 10-digit mobile number")]
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }

        [Display(Name = "Address")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Display(Name = "Playing Cricket?")]
        public bool PlayingCricket { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "12th Percentage")]
        [Range(0, 100, ErrorMessage = "Percentage must be between 0 and 100")]
        public decimal Percentage12th { get; set; }

        [Display(Name = "Live in Rajkot?")]
        public bool LiveInRajkot { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Created On")]
        public DateTime Created { get; set; }

        [Display(Name = "Modified On")]
        public DateTime Modified { get; set; }
    }
}
