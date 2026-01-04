using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IConfiguration _configuration;

        public DoctorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region DoctorList
        [SessionAuthorize("Admin")]
        public IActionResult DoctorList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Doctor_SelectAll";

            SqlDataReader reader = command.ExecuteReader();
            
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            
            return View(table);
        }
        #endregion

        #region DoctorAddEdit
        [SessionAuthorize("Admin")]
        [HttpGet]
        public IActionResult DoctorAddEdit(int? DoctorID)
        {
            UserDropDown();
            if (DoctorID != null && DoctorID > 0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Doctor_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DoctorID", DoctorID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DoctorModel doctor = new DoctorModel
                    {
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Qualification = reader["Qualification"].ToString(),
                        Specialization = reader["Specialization"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    };
                    connection.Close();
                    return View(doctor);
                }

                connection.Close();
            }

            return View(new DoctorModel());
        }

        [HttpPost]
        [SessionAuthorize("Admin")]
        public IActionResult DoctorAddEdit(DoctorModel doctorModel)
        {
            if (ModelState.IsValid)
            {
                UserDropDown();
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command;
                if (doctorModel.DoctorID > 0)
                {
                    command = new SqlCommand("PR_Doctor_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorID", doctorModel.DoctorID);
                }
                else
                {
                    command = new SqlCommand("PR_Doctor_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }

                command.Parameters.AddWithValue("@UserID", doctorModel.UserID);
                command.Parameters.AddWithValue("@Name", doctorModel.Name);
                command.Parameters.AddWithValue("@Email", doctorModel.Email);
                command.Parameters.AddWithValue("@Phone", doctorModel.Phone);
                command.Parameters.AddWithValue("@Qualification", doctorModel.Qualification);
                command.Parameters.AddWithValue("@Specialization", doctorModel.Specialization);
                command.Parameters.AddWithValue("@IsActive", doctorModel.IsActive);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "Doctor saved successfully.";
                return RedirectToAction("DoctorList");
            }

            return View(doctorModel);
        }
        #endregion

        #region DoctorDelete
        public IActionResult DoctorDelete(int ID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Doctor_DeleteByPK";
                cmd.Parameters.AddWithValue("@DoctorID", ID);

                cmd.ExecuteNonQuery();
                conn.Close();

                TempData["SuccessMessage"] = "Doctor deleted successfully.";
                return RedirectToAction("DoctorList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Delete failed: " + ex.Message;
                return RedirectToAction("DoctorList");
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
                    SqlCommand cmd = new SqlCommand("PR_Doctor_DeleteAll", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }

                TempData["Success"] = "All doctor-department records have been deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Delete All failed: " + ex.Message;
            }

            return RedirectToAction("DoctorList");
        }
        #endregion

        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Doctor_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DoctorList");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"DoctorList.xlsx";
                //string fileName = $"DoctorList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Doctor_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (MemoryStream stream = new MemoryStream())
            {
                // Create PDF Document
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Add title
                iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
                pdfDoc.Add(new iTextSharp.text.Paragraph("DoctorList", titleFont));
                pdfDoc.Add(new iTextSharp.text.Paragraph("\n"));

                // Create PDF table
                iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
                pdfTable.WidthPercentage = 100;

                // Add table header
                foreach (DataColumn column in dt.Columns)
                {
                    pdfTable.AddCell(new iTextSharp.text.Phrase(column.ColumnName));
                }

                // Add table rows
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var cell in row.ItemArray)
                    {
                        pdfTable.AddCell(cell?.ToString() ?? "");
                    }
                }

                pdfDoc.Add(pdfTable);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "DoctorList.pdf");
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

    }
}

