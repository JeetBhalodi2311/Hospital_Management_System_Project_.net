using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region DepartmentList
        [SessionAuthorize("Admin")]
        public IActionResult DepartmentList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Department_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region DepartmentAddEdit
        // GET: Add/Edit form
        [SessionAuthorize("Admin")]
        [HttpGet]
        public IActionResult DepartmentAddEdit(int? DepartmentID)
        {
            UserDropDown();
            if (DepartmentID != null && DepartmentID > 0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Department_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DepartmentID", DepartmentID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DepartmentModel department = new DepartmentModel
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        DepartmentName = reader["DepartmentName"].ToString(),
                        Description = reader["Description"].ToString(),
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    };
                    return View(department);
                }
            }

            return View(new DepartmentModel());
        }

        // POST: Insert or Update
        [SessionAuthorize("Admin")]
        [HttpPost]
        public IActionResult DepartmentAddEdit(DepartmentModel departmentModel)
        {
            if (ModelState.IsValid)
            {
                UserDropDown();
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command;
                if (departmentModel.DepartmentID > 0)
                {
                    command = new SqlCommand("PR_Department_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DepartmentID", departmentModel.DepartmentID);

                    command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }
                else
                {
                    command = new SqlCommand("PR_Department_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }

                command.Parameters.AddWithValue("@UserID", departmentModel.UserID);
                command.Parameters.AddWithValue("@DepartmentName", departmentModel.DepartmentName);
                command.Parameters.AddWithValue("@Description", departmentModel.Description);
                command.Parameters.AddWithValue("@IsActive", departmentModel.IsActive);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "Department saved successfully.";
                return RedirectToAction("DepartmentList");
            }

            return View(departmentModel);
        }
        #endregion

        #region DepartmentDelete
        public IActionResult DepartmentDelete(int ID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("HMS_Tables");

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Department_DeleteByPK";
                cmd.Parameters.AddWithValue("@DepartmentID", ID);

                cmd.ExecuteNonQuery();
                conn.Close();

                TempData["SuccessMessage"] = "Department deleted successfully.";
                return RedirectToAction("DepartmentList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Delete failed: " + e.Message;
                return RedirectToAction("DepartmentList");
                
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
                    SqlCommand cmd = new SqlCommand("PR_Department_DeleteAll", conn);
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

            return RedirectToAction("DepartmentList");
        }
        #endregion

        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Department_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DepartmentList");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"DepartmentList.xlsx";
                //string fileName = $"DepartmentList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Department_SelectAll", connection);
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
                pdfDoc.Add(new iTextSharp.text.Paragraph("DepartmentList", titleFont));
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

                return File(stream.ToArray(), "application/pdf", "DepartmentList.pdf");
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
