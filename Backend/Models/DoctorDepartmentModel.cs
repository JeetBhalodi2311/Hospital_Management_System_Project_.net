using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class DoctorDepartmentModel
    {
        public int DoctorDepartmentID { get; set; }

        [Required(ErrorMessage = "Doctor is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Doctor")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Department")]
        public int DepartmentID { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime Modified { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "User ID is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid User ID")]
        public int UserID { get; set; }
    }
}
