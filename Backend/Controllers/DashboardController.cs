using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IConfiguration _configuration;

        public DashboardController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [SessionAuthorize("Admin")]
        public IActionResult Index()
        {
            var model = new DashboardViewModel();

            using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
            {
                conn.Open();

                model.TotalUsers = GetCount(conn, "SELECT COUNT(*) FROM [User]");
                model.TotalDoctors = GetCount(conn, "SELECT COUNT(*) FROM Doctor");
                model.TotalDepartments = GetCount(conn, "SELECT COUNT(*) FROM Department");
                model.TotalDoctorDepartments = GetCount(conn, "SELECT COUNT(*) FROM DoctorDepartment");

                model.TotalPatients = GetCount(conn, "SELECT COUNT(*) FROM Patient");
                model.TotalAppointments = GetCount(conn, "SELECT COUNT(*) FROM Appointment");
            }

            return View(model);
        }
        [SessionAuthorize("Admin")]
        private int GetCount(SqlConnection conn, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
