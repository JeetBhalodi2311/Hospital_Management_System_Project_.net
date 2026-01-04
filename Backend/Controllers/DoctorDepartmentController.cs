using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class DoctorDepartmentController : Controller
    {
        private IConfiguration _configuration;

        public DoctorDepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region DoctorDepartmentList
        [SessionAuthorize("Admin")]
        public IActionResult DoctorDepartmentList()
        {
            SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_DoctorDepartment_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();
            return View(table);
        }
        #endregion

        #region DoctorDepartmentAddEdit
        // GET: Add/Edit form
        [SessionAuthorize("Admin")]
        [HttpGet]
        public IActionResult DoctorDepartmentAddEdit(int? DoctorDepartmentID)
        {
            UserDropDown();
            DoctorDropDown();
            DepartmentDropDown();
            if (DoctorDepartmentID != null && DoctorDepartmentID > 0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_DoctorDepartment_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;   
                command.Parameters.AddWithValue("@DoctorDepartmentID", DoctorDepartmentID);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    DoctorDepartmentModel doctorDepartment = new DoctorDepartmentModel
                    {
                        DoctorDepartmentID = Convert.ToInt32(reader["DoctorDepartmentID"]),
                        DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                        DoctorID = Convert.ToInt32(reader["DoctorID"]),
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Created = Convert.ToDateTime(reader["Created"]),
                        Modified = Convert.ToDateTime(reader["Modified"])
                    };
                    connection.Close();
                    return View(doctorDepartment);
                }

                connection.Close();
            }

            return View(new DoctorDepartmentModel());
        }

        // POST: Insert or Update
        [SessionAuthorize("Admin")]
        [HttpPost]
        public IActionResult DoctorDepartmentAddEdit(DoctorDepartmentModel doctorDepartmentModel)
        {
            if (ModelState.IsValid)
            {
                UserDropDown();
                DoctorDropDown();
                DepartmentDropDown();
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command;
                if (doctorDepartmentModel.DoctorDepartmentID > 0)
                {
                    command = new SqlCommand("PR_DoctorDepartment_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DoctorDepartmentID", doctorDepartmentModel.DoctorDepartmentID);
                }
                else
                {
                    command = new SqlCommand("PR_DoctorDepartment_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }

                command.Parameters.AddWithValue("@UserID", doctorDepartmentModel.UserID);
                command.Parameters.AddWithValue("@DepartmentID", doctorDepartmentModel.DepartmentID);
                command.Parameters.AddWithValue("@DoctorID", doctorDepartmentModel.DoctorID);
                command.Parameters.AddWithValue("@Created", DateTime.Now);
                command.Parameters.AddWithValue("@Modified", DateTime.Now);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "DoctorDepartment saved successfully.";
                return RedirectToAction("DoctorDepartmentList");
            }

            return View(doctorDepartmentModel);
        }
        #endregion

        #region DoctorDeparmentDelete
        public IActionResult DoctorDeparmentDelete(int ID)
        {
            try
            {
                string connectionString = this._configuration.GetConnectionString("HMS_Tables");

                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_DoctorDepartment_DeleteByPK";
                cmd.Parameters.AddWithValue("@DOCTORDEPARTMENTID", ID);


                cmd.ExecuteNonQuery();
                conn.Close();


                TempData["SuccessMessage"] = "DoctorDepartment deleted successfully.";
                return RedirectToAction("DoctorDepartmentList");
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = "Delete failed: " + e.Message;
                return RedirectToAction("DoctorDepartmentList");

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
                    SqlCommand cmd = new SqlCommand("PR_DoctorDepartment_DeleteAll", conn);
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

            return RedirectToAction("DoctorDepartmentList");
        }
        #endregion

       
        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_DoctorDepartment_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DoctorDepartmentList");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"DoctorDepartmentList.xlsx";

                //string fileName = $"DoctorDepartmentList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_DoctorDepartment_SelectAll", connection);
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
                pdfDoc.Add(new iTextSharp.text.Paragraph("DoctorDepartmentList", titleFont));
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

                return File(stream.ToArray(), "application/pdf", "DoctorDepartmentList.pdf");
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
                command.CommandText = "PR_Doctor_SelectForDropDown"; 

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

        public void DepartmentDropDown()
        {
            string connectionString = this._configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "PR_Department_SelectForDropDown";

                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);

                List<DepartmentDropDownModel> departmentList = new List<DepartmentDropDownModel>();
                foreach (DataRow data in dataTable.Rows)
                {
                    DepartmentDropDownModel model = new DepartmentDropDownModel();
                    model.DepartmentID = Convert.ToInt32(data["DepartmentID"]);
                    model.DepartmentName = data["DepartmentName"].ToString();
                    departmentList.Add(model);
                }
                ViewBag.DepartmentList = departmentList;
            }
        }



    }
}
