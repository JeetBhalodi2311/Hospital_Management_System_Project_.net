using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Hospital_Management_System.Helpers;

using OfficeOpenXml;


namespace Hospital_Management_System.Controllers
{
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region UserList

        //public IActionResult UserList()
        //{
        //    string? role = HttpContext.Session.GetString("UserRole");
        //    string? name = HttpContext.Session.GetString("UserName");

        //    if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(name))
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }


        //    using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
        //    {
        //        connection.Open();

        //        SqlCommand command = new SqlCommand("PR_User_SelectAll", connection);
        //        command.CommandType = CommandType.StoredProcedure;

        //        SqlDataReader reader = command.ExecuteReader();
        //        DataTable table = new DataTable();
        //        table.Load(reader);
        //        connection.Close();

        //        return View(table);
        //    }
        //}
        [SessionAuthorize("Admin")]
        public IActionResult UserList()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_User_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            connection.Close();

            return View(table);
        }
        #endregion

        #region UserAddEdit
        // GET: Add/Edit form

        //[HttpGet]
        //[SessionAuthorize("Admin")]
        //public IActionResult UserAddEdit(int? UserID)
        //{
        //    if (UserID != null && UserID > 0)
        //    {
        //        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
        //        connection.Open();

        //        SqlCommand command = new SqlCommand("PR_User_SelectByPK", connection);
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@UserID", UserID);

        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            UserModel user = new UserModel
        //            {
        //                UserID = Convert.ToInt32(reader["UserID"]),
        //                UserName = reader["UserName"].ToString(),
        //                Password = reader["Password"].ToString(),
        //                Email = reader["Email"].ToString(),
        //                MobileNo = reader["MobileNo"].ToString(),
        //                IsActive = Convert.ToBoolean(reader["IsActive"]),
        //                Created = Convert.ToDateTime(reader["Created"]),
        //                Modified = Convert.ToDateTime(reader["Modified"])
        //            };
        //            return View(user);
        //        }
        //    }

        //    return View(new UserModel());
        //}

        [HttpGet]
        [SessionAuthorize("Admin")]
        public IActionResult UserAddEdit(string? key)
        {
            int? UserID = null;

            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    UserID = Hospital_Management_System.Helpers.IdEncoder.Decode(key);
                }
                catch
                {
                    TempData["ErrorMessage"] = "Invalid user key.";
                    return RedirectToAction("UserList");
                }
            }

            if (UserID != null && UserID > 0)
            {
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("PR_User_SelectByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", UserID);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        UserModel user = new UserModel
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            UserName = reader["UserName"].ToString(),
                            Password = reader["Password"].ToString(),
                            Email = reader["Email"].ToString(),
                            MobileNo = reader["MobileNo"].ToString(),
                            IsActive = Convert.ToBoolean(reader["IsActive"]),
                            Created = Convert.ToDateTime(reader["Created"]),
                            Modified = Convert.ToDateTime(reader["Modified"])
                        };
                        return View(user);
                    }
                }
            }

            return View(new UserModel());
        }


        // POST: Insert or Update
        [HttpPost]
        [SessionAuthorize("Admin")]
        public IActionResult UserAddEdit(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();
 
                SqlCommand command;
                if (userModel.UserID > 0)
                {
                    command = new SqlCommand("PR_User_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userModel.UserID);

                    //command.Parameters.AddWithValue("@Created", DateTime.Now);
                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }
                else
                {
                    command = new SqlCommand("PR_User_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@Created", DateTime.Now);

                    command.Parameters.AddWithValue("@Modified", DateTime.Now);
                }

                command.Parameters.AddWithValue("@UserName", userModel.UserName);
                command.Parameters.AddWithValue("@Password", userModel.Password);
                command.Parameters.AddWithValue("@Created", DateTime.Now);

                command.Parameters.AddWithValue("@Email", userModel.Email);
                command.Parameters.AddWithValue("@MobileNo", userModel.MobileNo);
                command.Parameters.AddWithValue("@IsActive", userModel.IsActive);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "User saved successfully.";
                return RedirectToAction("UserList");

                //TempData["SuccessMessage"] = "User saved successfully.";
                //return RedirectToAction("UserAddEdit", new { key = IdEncoder.Encode(userModel.UserID) });

            }

            return View(userModel);
        }
        #endregion

        #region UserDelete
        public IActionResult UserDelete(int ID)
        {
            try
            {
                SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                conn.Open();

                SqlCommand cmd = new SqlCommand("PR_User_DeleteByPK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", ID);

                cmd.ExecuteNonQuery();
                conn.Close(); 

                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Delete failed: " + ex.Message;
                return RedirectToAction("UserList");
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
                    SqlCommand cmd = new SqlCommand("PR_User_DeleteAll", conn);
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

            return RedirectToAction("UserList");
        }
        #endregion

        #region Export to Excel
        public IActionResult ExportToExcel()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_User_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            connection.Close();

            using (var package = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("UserList");

                worksheet.Cells["A1"].LoadFromDataTable(dt, true);

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = $"UserList.xlsx";

                //string fileName = $"UserList_{DateTime.Now:yyyy-MM-dd||HH:mm:ss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
        #endregion

        #region Export to PDF
        public IActionResult ExportToPDF()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_User_SelectAll", connection);
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
                pdfDoc.Add(new iTextSharp.text.Paragraph("UserList", titleFont));
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

                return File(stream.ToArray(), "application/pdf", "UserList.pdf");
            }
        }
        #endregion

    }
}
