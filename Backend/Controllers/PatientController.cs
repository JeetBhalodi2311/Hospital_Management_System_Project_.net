using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class PatientController : Controller
    {
        private IConfiguration _configuration;

        public PatientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region PatientList
        [SessionAuthorize("Admin")]
        public IActionResult PatientList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Patient_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region PatientAddEdit
        // GET: Add/Edit form
        [SessionAuthorize("Admin")]
        [HttpGet]
        public IActionResult PatientAddEdit(int? PatientID)
        {
            UserDropDown();
            if (PatientID != null && PatientID > 0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Patient_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PatientID", PatientID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PatientModel patient = new PatientModel
                    {
                        PatientID = Convert.ToInt32(reader["PatientID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Name = reader["Name"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Gender = reader["Gender"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Address = reader["Address"].ToString(),
                        City = reader["City"].ToString(),
                        State = reader["State"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    };
                    connection.Close();
                    return View(patient);
                }

                connection.Close();
            }

            return View(new PatientModel());
        }

        // POST: Insert or Update
        [SessionAuthorize("Admin")]
        [HttpPost]
        public IActionResult PatientAddEdit(PatientModel patientModel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command;
                if (patientModel.PatientID > 0)
                {
                    command = new SqlCommand("PR_Patient_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PatientID", patientModel.PatientID);
                }
                else
                {
                    command = new SqlCommand("PR_Patient_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }

                command.Parameters.AddWithValue("@UserID", patientModel.UserID);
                command.Parameters.AddWithValue("@Name", patientModel.Name);
                command.Parameters.AddWithValue("@DateOfBirth", patientModel.DateOfBirth);
                command.Parameters.AddWithValue("@Gender", patientModel.Gender);
                command.Parameters.AddWithValue("@Email", patientModel.Email);
                command.Parameters.AddWithValue("@Phone", patientModel.Phone);
                command.Parameters.AddWithValue("@Address", patientModel.Address);
                command.Parameters.AddWithValue("@City", patientModel.City);
                command.Parameters.AddWithValue("@State", patientModel.State);
                command.Parameters.AddWithValue("@IsActive", patientModel.IsActive);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "Patient is saved successfullyy..";
                return RedirectToAction("PatientList");
            }

            return View(patientModel);
        }
        #endregion

        #region PatientDelete
        public IActionResult PatientDelete(int ID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("HMS_Tables");

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Patient_DeleteByPK";
                cmd.Parameters.AddWithValue("@PatientID", ID);

                cmd.ExecuteNonQuery();
                conn.Close();


                TempData["SuccessMessage"] = "Patient deleted successfully.";
                return RedirectToAction("PatientList");
            }
            catch (Exception e)
            {

                TempData["ErrorMessage"] = "Delete failed: " + e.Message;
                return RedirectToAction("PatientList");
               
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
                    SqlCommand cmd = new SqlCommand("PR_Patient_DeleteAll", conn);
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

            return RedirectToAction("PatientList");
        }
        #endregion

        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Patient_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("PatientList");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"PatientList.xlsx";

                //string fileName = $"PatientList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Patient_SelectAll", connection);
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
                pdfDoc.Add(new iTextSharp.text.Paragraph("PatientList", titleFont));
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

                return File(stream.ToArray(), "application/pdf", "PatientList.pdf");
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
