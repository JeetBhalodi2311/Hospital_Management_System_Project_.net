using System;
using System.ComponentModel.DataAnnotations;

namespace Hospital_Management_System.Models
{
    public class AppointmentModel
    {
        public int AppointmentID { get; set; }

        [Required(ErrorMessage = "Doctor ID is required")]
        public int DoctorID { get; set; }

        [Required(ErrorMessage = "Patient ID is required")]
        public int PatientID { get; set; }

        [Required(ErrorMessage = "Appointment Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime? AppointmentDate { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string AppointmentStatus { get; set; }

        public string Description { get; set; }

        public string SpecialRemarks { get; set; }

        public decimal? TotalConsultedAmount { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public int UserID { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? Created { get; set; }   // handled in controller
        public DateTime? Modified { get; set; }  // handled in controller
    }
}
