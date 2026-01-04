//using Hospital_Management_System.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using System.Data.SqlClient;

//namespace Hospital_Management_System.Controllers
//{
//    public class AppointmentController : Controller
//    {
//        private IConfiguration _configuration;

//        public AppointmentController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        #region AppointmentList
//        public IActionResult AppointmentList()
//        {
//            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables"));
//            connection.Open();

//            SqlCommand command = connection.CreateCommand();
//            command.CommandType = CommandType.StoredProcedure;
//            command.CommandText = "PR_Appointment_SelectAll";
//            SqlDataReader reader = command.ExecuteReader();
//            DataTable table = new DataTable();
//            table.Load(reader);
//            connection.Close();
//            return View(table);
//        }
//        #endregion

//        #region AppointmentAddEdit
//        // GET: Add/Edit form
//        [HttpGet]
//        public IActionResult AppointmentAddEdit(int? AppointmentID)
//        {
//            if (AppointmentID != null && AppointmentID > 0)
//            {
//                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//                connection.Open();

//                SqlCommand command = new SqlCommand("PR_Appointment_SelectByPK", connection);
//                command.CommandType = CommandType.StoredProcedure;
//                command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

//                SqlDataReader reader = command.ExecuteReader();
//                if (reader.Read())
//                {
//                    AppointmentModel appointment = new AppointmentModel
//                    {
//                        PatientID = Convert.ToInt32(reader["PatientID"]),
//                        AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
//                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
//                        UserID = Convert.ToInt32(reader["UserID"]),
//                        AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
//                        AppointmentStatus = reader["AppointmentStatus"].ToString(),
//                        Description = reader["Description"].ToString(),
//                        SpecialRemarks = reader["SpecialRemarks"].ToString(),
//                        TotalConsultedAmount = Convert.ToDecimal(reader["TotalConsultedAmount"]),
//                        Created = Convert.ToDateTime(reader["Created"]),
//                        Modified = Convert.ToDateTime(reader["Modified"])
//                    };
//                    connection.Close();
//                    return View(appointment);
//                }

//                connection.Close();
//            }

//            return View(new AppointmentModel());
//        }

//        // POST: Insert or Update
//        [HttpPost]
//        public IActionResult AppointmentAddEdit(AppointmentModel appointmentModel)
//        {
//            if (ModelState.IsValid)
//            {
//                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//                connection.Open();

//                SqlCommand command;
//                if (appointmentModel.AppointmentID > 0)
//                {
//                    command = new SqlCommand("PR_Appointment_UpdateByPK", connection);
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@AppointmentID", appointmentModel.AppointmentID);
//                }
//                else
//                {
//                    command = new SqlCommand("PR_Appointment_Insert", connection);
//                    command.CommandType = CommandType.StoredProcedure;
//                }

//                //command.Parameters.AddWithValue("@AppointmentID", appointmentModel.AppointmentID);

//                command.Parameters.AddWithValue("@DoctorID", appointmentModel.DoctorID);
//                command.Parameters.AddWithValue("@PatientID", appointmentModel.PatientID);
//                command.Parameters.AddWithValue("@UserID", appointmentModel.UserID);
//                command.Parameters.AddWithValue("@AppointmentDate", appointmentModel.AppointmentDate);
//                command.Parameters.AddWithValue("@AppointmentStatus", appointmentModel.AppointmentStatus);
//                command.Parameters.AddWithValue("@Description", appointmentModel.Description);
//                command.Parameters.AddWithValue("@SpecialRemarks", appointmentModel.SpecialRemarks);
//                command.Parameters.AddWithValue("@TotalConsultedAmount", appointmentModel.TotalConsultedAmount);
//                command.Parameters.AddWithValue("@Created", DateTime.Now);
//                command.Parameters.AddWithValue("@Modified", DateTime.Now);

//                command.ExecuteNonQuery();
//                connection.Close();

//                TempData["SuccessMessage"] = "Appointment saved successfully.";
//                return RedirectToAction("AppointmentList");
//            }

//            return View(appointmentModel);
//        }

//        #endregion

//        #region AppointmentDelete
//        public IActionResult AppointmentDelete(int ID)
//        {
//            try
//            {
//                string connectionString = this._configuration.GetConnectionString("HMS_Tables");

//                SqlConnection conn = new SqlConnection(connectionString);
//                conn.Open();

//                SqlCommand cmd = conn.CreateCommand();
//                cmd.CommandType = CommandType.StoredProcedure;
//                cmd.CommandText = "PR_Appointment_DeleteByPK";
//                cmd.Parameters.AddWithValue("@AppointmentID", ID);

//                cmd.ExecuteNonQuery();
//                conn.Close();

//                return RedirectToAction("AppointmentList");
//            }
//            catch (Exception e)
//            {
//                TempData["Error"] = "Error";
//                return RedirectToAction("AppointmentList");

//            }
//        }
//        #endregion

//        #region Delete All
//        public IActionResult DeleteAll()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("PR_Appointment_DeleteAll", conn);
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.ExecuteNonQuery();
//                    conn.Close();
//                }

//                TempData["Success"] = "All doctor-department records have been deleted.";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "Delete All failed: " + ex.Message;
//            }

//            return RedirectToAction("AppointmentList");
//        }
//        #endregion

//        #region Export to Excel
//        public IActionResult ExportToExcel()
//        {
//            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//            connection.Open();

//            SqlCommand command = new SqlCommand("PR_Appointment_SelectAll", connection);
//            command.CommandType = CommandType.StoredProcedure;

//            SqlDataReader reader = command.ExecuteReader();
//            DataTable dt = new DataTable();
//            dt.Load(reader);
//            connection.Close();

//            using (var package = new OfficeOpenXml.ExcelPackage())
//            {
//                var worksheet = package.Workbook.Worksheets.Add("AppointmentList");

//                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

//                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

//                var stream = new MemoryStream();
//                package.SaveAs(stream);
//                stream.Position = 0;

//                string fileName = $"AppointmentList.xlsx";
//                //string fileName = $"AppointmentList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
//                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
//            }
//        }
//        #endregion

//        #region Export to PDF
//        public IActionResult ExportToPDF()
//        {
//            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//            connection.Open();

//            SqlCommand command = new SqlCommand("PR_Appointment_SelectAll", connection);
//            command.CommandType = CommandType.StoredProcedure;

//            SqlDataReader reader = command.ExecuteReader();
//            DataTable dt = new DataTable();
//            dt.Load(reader);
//            connection.Close();

//            using (MemoryStream stream = new MemoryStream())
//            {
//                // Create PDF Document
//                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
//                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
//                pdfDoc.Open();

//                // Add title
//                iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
//                pdfDoc.Add(new iTextSharp.text.Paragraph("Appointment List", titleFont));
//                pdfDoc.Add(new iTextSharp.text.Paragraph("\n"));

//                // Create PDF table
//                iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
//                pdfTable.WidthPercentage = 100;

//                // Add table header
//                foreach (DataColumn column in dt.Columns)
//                {
//                    pdfTable.AddCell(new iTextSharp.text.Phrase(column.ColumnName));
//                }

//                // Add table rows
//                foreach (DataRow row in dt.Rows)
//                {
//                    foreach (var cell in row.ItemArray)
//                    {
//                        pdfTable.AddCell(cell?.ToString() ?? "");
//                    }
//                }

//                pdfDoc.Add(pdfTable);
//                pdfDoc.Close();

//                return File(stream.ToArray(), "application/pdf", "AppointmentList.pdf");
//            }
//        }
//        #endregion

//    }
//}


using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class AppointmentController : Controller
    {
        private IConfiguration _configuration;

        public AppointmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region AppointmentList
        [SessionAuthorize("Admin")]
        public IActionResult AppointmentList(string? filter)
        {
            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables")))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Appointment_SelectAll";
                command.Parameters.AddWithValue("@Filter", string.IsNullOrEmpty(filter) ? DBNull.Value : (object)filter);
                SqlDataReader reader = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return View(table);
            }
        }
        #endregion

        #region AppointmentAddEdit
        // GET: Add/Edit form
        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult AppointmentAddEdit(int? AppointmentID)
        {
            UserDropDown();
            PatientDropDown();
            DoctorDropDown();
            if (AppointmentID != null && AppointmentID > 0)
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("PR_Appointment_SelectByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        AppointmentModel appointment = new AppointmentModel
                        {
                            AppointmentID = Convert.ToInt32(reader["AppointmentID"]),
                            DoctorID = Convert.ToInt32(reader["DoctorID"]),
                            PatientID = Convert.ToInt32(reader["PatientID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            AppointmentDate = Convert.ToDateTime(reader["AppointmentDate"]),
                            AppointmentStatus = reader["AppointmentStatus"].ToString(),
                            Description = reader["Description"]?.ToString(),
                            SpecialRemarks = reader["SpecialRemarks"]?.ToString(),
                            TotalConsultedAmount = reader["TotalConsultedAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalConsultedAmount"]) : null,
                            Created = reader["Created"] != DBNull.Value ? Convert.ToDateTime(reader["Created"]) : null,
                            Modified = reader["Modified"] != DBNull.Value ? Convert.ToDateTime(reader["Modified"]) : null
                        };
                        return View(appointment);
                    }
                }
            }

            return View(new AppointmentModel());
        }

        // POST: Insert or Update
        [HttpPost]
        [SessionAuthorize("Admin")]
        public IActionResult AppointmentAddEdit(AppointmentModel appointmentModel)
        {
            if (!ModelState.IsValid)
            {
                UserDropDown();
                PatientDropDown();
                DoctorDropDown();
                return View(appointmentModel);
            }

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
            {
                connection.Open();

                SqlCommand command;
                if (appointmentModel.AppointmentID > 0)
                {
                    // Update
                    command = new SqlCommand("PR_Appointment_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@AppointmentID", appointmentModel.AppointmentID);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }
                else
                {
                    // Insert
                    command = new SqlCommand("PR_Appointment_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }

                // Common parameters
                command.Parameters.AddWithValue("@DoctorID", appointmentModel.DoctorID);
                command.Parameters.AddWithValue("@PatientID", appointmentModel.PatientID);
                command.Parameters.AddWithValue("@UserID", appointmentModel.UserID);
                command.Parameters.AddWithValue("@AppointmentDate", appointmentModel.AppointmentDate);
                command.Parameters.AddWithValue("@AppointmentStatus", appointmentModel.AppointmentStatus);
                command.Parameters.AddWithValue("@Description", appointmentModel.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@SpecialRemarks", appointmentModel.SpecialRemarks ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@TotalConsultedAmount", appointmentModel.TotalConsultedAmount ?? (object)DBNull.Value);

                command.ExecuteNonQuery();
            }

            TempData["SuccessMessage"] = "Appointment saved successfully.";
            return RedirectToAction("AppointmentList");
        }
        #endregion

        #region AppointmentDelete
        public IActionResult AppointmentDelete(int ID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
                {
                    conn.Open();

                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PR_Appointment_DeleteByPK";
                    cmd.Parameters.AddWithValue("@AppointmentID", ID);

                    cmd.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Appointment deleted successfully.";
                return RedirectToAction("AppointmentList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Delete failed: " + e.Message;
                return RedirectToAction("AppointmentList");
            }
        }
        #endregion

        #region Delete All
        public IActionResult DeleteAll()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("PR_Appointment_DeleteAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                TempData["Success"] = "All appointment records have been deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Delete All failed: " + ex.Message;
            }

            return RedirectToAction("AppointmentList");
        }
        #endregion

        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Appointment_SelectAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                using (var package = new OfficeOpenXml.ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("AppointmentList");
                    worksheet.Cells["A1"].LoadFromDataTable(dt, true);
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                    var stream = new MemoryStream();
                    package.SaveAs(stream);
                    stream.Position = 0;

                    string fileName = $"AppointmentList.xlsx";
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Appointment_SelectAll", connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                using (MemoryStream stream = new MemoryStream())
                {
                    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
                    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                    pdfDoc.Add(new iTextSharp.text.Paragraph("Appointment List", titleFont));
                    pdfDoc.Add(new iTextSharp.text.Paragraph("\n"));

                    iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
                    pdfTable.WidthPercentage = 100;

                    foreach (DataColumn column in dt.Columns)
                    {
                        pdfTable.AddCell(new iTextSharp.text.Phrase(column.ColumnName));
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var cell in row.ItemArray)
                        {
                            pdfTable.AddCell(cell?.ToString() ?? "");
                        }
                    }

                    pdfDoc.Add(pdfTable);
                    pdfDoc.Close();

                    return File(stream.ToArray(), "application/pdf", "AppointmentList.pdf");
                }
            }
        }
        #endregion

        public void UserDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_User_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<UserDropDownModel> userList = new List<UserDropDownModel>();
                foreach (DataRow data in dataTable.Rows)
                {
                    UserDropDownModel model = new UserDropDownModel();
                    model.UserID = Convert.ToInt32(data["UserID"]);
                    model.UserName = data["UserName"].ToString();
                    userList.Add(model);
                }
                ViewBag.UserList = userList;
            }
        }

        public void DoctorDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Doctor_SelectForDropDown";  // You need similar SP for Doctor

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<DoctorDropDownModel> doctorList = new List<DoctorDropDownModel>();
                foreach (DataRow data in dataTable.Rows)
                {
                    DoctorDropDownModel model = new DoctorDropDownModel();
                    model.DoctorID = Convert.ToInt32(data["DoctorID"]);
                    model.Name = data["Name"].ToString();
                    doctorList.Add(model);
                }
                ViewBag.DoctorList = doctorList;
            }
        }

        public void PatientDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Patient_SelectForDropDown";  // Need SP for Patient

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<PatientDropDownModel> patientList = new List<PatientDropDownModel>();
                foreach (DataRow data in dataTable.Rows)
                {
                    PatientDropDownModel model = new PatientDropDownModel();
                    model.PatientID = Convert.ToInt32(data["PatientID"]);
                    model.Name = data["Name"].ToString();
                    patientList.Add(model);
                }
                ViewBag.PatientList = patientList;
            }
        }
    }
}
