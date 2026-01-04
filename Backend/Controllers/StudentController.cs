//using Hospital_Management_System.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;
//using System.Data.SqlClient;
//using Hospital_Management_System.Helpers;
//using OfficeOpenXml;

//namespace Hospital_Management_System.Controllers
//{
//    public class StudentController : Controller
//    {
//        private readonly IConfiguration _configuration;

//        public StudentController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        #region StudentList
//        [SessionAuthorize("Admin")]
//        public IActionResult StudentList()
//        {
//            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//            {
//                connection.Open();
//                SqlCommand command = new SqlCommand("PR_Student_SelectAll", connection);
//                command.CommandType = CommandType.StoredProcedure;

//                SqlDataReader reader = command.ExecuteReader();
//                DataTable table = new DataTable();
//                table.Load(reader);
//                connection.Close();

//                return View(table);
//            }
//        }
//        #endregion

//        #region StudentAddEdit (GET)
//        [HttpGet]
//        [SessionAuthorize("Admin")]
//        public IActionResult StudentAddEdit(string? key)
//        {
//            int? StudentID = null;

//            if (!string.IsNullOrEmpty(key))
//            {
//                try
//                {
//                    StudentID = IdEncoder.Decode(key);
//                }
//                catch
//                {
//                    TempData["ErrorMessage"] = "Invalid student key.";
//                    return RedirectToAction("StudentList");
//                }
//            }

//            if (StudentID != null && StudentID > 0)
//            {
//                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//                {
//                    connection.Open();

//                    SqlCommand command = new SqlCommand("PR_Student_SelectByPK", connection);
//                    command.CommandType = CommandType.StoredProcedure;
//                    command.Parameters.AddWithValue("@StudentID", StudentID);

//                    SqlDataReader reader = command.ExecuteReader();
//                    if (reader.Read())
//                    {
//                        StudentModel student = new StudentModel
//                        {
//                            StudentID = Convert.ToInt32(reader["StudentID"]),
//                            EnrollmentNo = reader["EnrollmentNo"].ToString(),
//                            Name = reader["Name"].ToString(),
//                            MobileNo = reader["MobileNo"].ToString(),
//                            Address = reader["Address"].ToString(),
//                            Email = reader["Email"].ToString(),
//                            Gender = reader["Gender"].ToString(),
//                            PlayingCricket = Convert.ToBoolean(reader["PlayingCricket"]),
//                            Password = reader["Password"].ToString(),
//                            ConfirmPassword = reader["ConfirmPassword"].ToString(),
//                            Percentage12th = reader["Percentage12th"] != DBNull.Value ? Convert.ToDecimal(reader["Percentage12th"]) : 0,
//                            LiveInRajkot = Convert.ToBoolean(reader["LiveInRajkot"]),
//                            IsActive = Convert.ToBoolean(reader["IsActive"]),
//                            Created = Convert.ToDateTime(reader["Created"]),
//                            Modified = Convert.ToDateTime(reader["Modified"])
//                        };
//                        return View(student);
//                    }
//                }
//            }

//            return View(new StudentModel());
//        }
//        #endregion

//        #region StudentAddEdit (POST)
//        [HttpPost]
//        [SessionAuthorize("Admin")]
//        public IActionResult StudentAddEdit(StudentModel studentModel)
//        {
//            if (ModelState.IsValid)
//            {
//                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//                {
//                    connection.Open();
//                    SqlCommand command;

//                    if (studentModel.StudentID > 0)
//                    {
//                        command = new SqlCommand("PR_Student_UpdateByPK", connection);
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@StudentID", studentModel.StudentID);
//                        command.Parameters.AddWithValue("@Modified", DateTime.Now);
//                    }
//                    else
//                    {
//                        command = new SqlCommand("PR_Student_Insert", connection);
//                        command.CommandType = CommandType.StoredProcedure;
//                        command.Parameters.AddWithValue("@Created", DateTime.Now);
//                        command.Parameters.AddWithValue("@Modified", DateTime.Now);
//                    }

//                    command.Parameters.AddWithValue("@EnrollmentNo", studentModel.EnrollmentNo);
//                    command.Parameters.AddWithValue("@Name", studentModel.Name);
//                    command.Parameters.AddWithValue("@MobileNo", studentModel.MobileNo);
//                    command.Parameters.AddWithValue("@Address", (object?)studentModel.Address ?? DBNull.Value);
//                    command.Parameters.AddWithValue("@Email", studentModel.Email);
//                    command.Parameters.AddWithValue("@Gender", studentModel.Gender);
//                    command.Parameters.AddWithValue("@PlayingCricket", studentModel.PlayingCricket);
//                    command.Parameters.AddWithValue("@Password", studentModel.Password);
//                    command.Parameters.AddWithValue("@ConfirmPassword", studentModel.ConfirmPassword);
//                    command.Parameters.AddWithValue("@Percentage12th", studentModel.Percentage12th);
//                    command.Parameters.AddWithValue("@LiveInRajkot", studentModel.LiveInRajkot);
//                    command.Parameters.AddWithValue("@IsActive", studentModel.IsActive);

//                    command.ExecuteNonQuery();
//                    connection.Close();

//                    TempData["SuccessMessage"] = "Student saved successfully.";
//                    return RedirectToAction("StudentList");
//                }
//            }

//            return View(studentModel);
//        }
//        #endregion

//        #region StudentDelete
//        public IActionResult StudentDelete(int ID)
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("PR_Student_DeleteByPK", conn);
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.Parameters.AddWithValue("@StudentID", ID);
//                    cmd.ExecuteNonQuery();
//                    conn.Close();
//                }

//                TempData["SuccessMessage"] = "Student deleted successfully.";
//                return RedirectToAction("StudentList");
//            }
//            catch (Exception ex)
//            {
//                TempData["ErrorMessage"] = "Delete failed: " + ex.Message;
//                return RedirectToAction("StudentList");
//            }
//        }
//        #endregion

//        #region DeleteAll
//        public IActionResult DeleteAll()
//        {
//            try
//            {
//                using (SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("HMS_Tables")))
//                {
//                    conn.Open();
//                    SqlCommand cmd = new SqlCommand("PR_Student_DeleteAll", conn);
//                    cmd.CommandType = CommandType.StoredProcedure;
//                    cmd.ExecuteNonQuery();
//                    conn.Close();
//                }

//                TempData["Success"] = "All student records have been deleted.";
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "Delete All failed: " + ex.Message;
//            }

//            return RedirectToAction("StudentList");
//        }
//        #endregion

//        #region ExportToExcel
//        public IActionResult ExportToExcel()
//        {
//            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//            connection.Open();
//            SqlCommand command = new SqlCommand("PR_Student_SelectAll", connection);
//            command.CommandType = CommandType.StoredProcedure;

//            SqlDataReader reader = command.ExecuteReader();
//            DataTable dt = new DataTable();
//            dt.Load(reader);
//            connection.Close();

//            using (var package = new ExcelPackage())
//            {
//                var worksheet = package.Workbook.Worksheets.Add("StudentList");
//                worksheet.Cells["A1"].LoadFromDataTable(dt, true);
//                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

//                var stream = new MemoryStream();
//                package.SaveAs(stream);
//                stream.Position = 0;

//                string fileName = $"StudentList.xlsx";
//                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
//            }
//        }
//        #endregion

//        #region ExportToPDF
//        public IActionResult ExportToPDF()
//        {
//            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
//            connection.Open();
//            SqlCommand command = new SqlCommand("PR_Student_SelectAll", connection);
//            command.CommandType = CommandType.StoredProcedure;

//            SqlDataReader reader = command.ExecuteReader();
//            DataTable dt = new DataTable();
//            dt.Load(reader);
//            connection.Close();

//            using (MemoryStream stream = new MemoryStream())
//            {
//                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 10f, 10f);
//                iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
//                pdfDoc.Open();

//                iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont("Arial", 16, iTextSharp.text.Font.BOLD);
//                pdfDoc.Add(new iTextSharp.text.Paragraph("Student List", titleFont));
//                pdfDoc.Add(new iTextSharp.text.Paragraph("\n"));

//                iTextSharp.text.pdf.PdfPTable pdfTable = new iTextSharp.text.pdf.PdfPTable(dt.Columns.Count);
//                pdfTable.WidthPercentage = 100;

//                foreach (DataColumn column in dt.Columns)
//                {
//                    pdfTable.AddCell(new iTextSharp.text.Phrase(column.ColumnName));
//                }

//                foreach (DataRow row in dt.Rows)
//                {
//                    foreach (var cell in row.ItemArray)
//                    {
//                        pdfTable.AddCell(cell?.ToString() ?? "");
//                    }
//                }

//                pdfDoc.Add(pdfTable);
//                pdfDoc.Close();

//                return File(stream.ToArray(), "application/pdf", "StudentList.pdf");
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
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;

        public StudentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Student List
        public IActionResult StudentList()
        {
            string connStr = _configuration.GetConnectionString("HMS_Tables");
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("PR_Student_SelectAll", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return View(dt);
        }

        // Add/Edit Page
        public IActionResult StudentAddEdit(int? StudentID)
        {
            if (StudentID == null)
                return View(new StudentModel());

            string connStr = _configuration.GetConnectionString("HMS_Tables");
            StudentModel student = new StudentModel();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("PR_Student_SelectByPK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", StudentID);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    student.StudentID = Convert.ToInt32(reader["StudentID"]);
                    student.EnrollmentNo = reader["EnrollmentNo"].ToString();
                    student.Name = reader["Name"].ToString();
                    student.MobileNo = reader["MobileNo"].ToString();
                    student.Address = reader["Address"].ToString();
                    student.Email = reader["Email"].ToString();
                    student.Gender = reader["Gender"].ToString();
                    student.PlayingCricket = Convert.ToBoolean(reader["PlayingCricket"]);
                    student.Password = reader["Password"].ToString();
                    student.ConfirmPassword = reader["ConfirmPassword"].ToString();
                    student.Percentage12th = Convert.ToDecimal(reader["Percentage12th"]);
                    student.LiveInRajkot = Convert.ToBoolean(reader["LiveInRajkot"]);
                    student.IsActive = Convert.ToBoolean(reader["IsActive"]);
                }
            }
            return View(student);
        }

        // Save Student
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudentSave(StudentModel model)
        {
            if (!ModelState.IsValid)
                return View("StudentAddEdit", model);

            string connStr = _configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand
                {
                    Connection = conn,
                    CommandType = CommandType.StoredProcedure
                };

                if (model.StudentID == 0)
                {
                    cmd.CommandText = "PR_Student_Insert";
                }
                else
                {
                    cmd.CommandText = "PR_Student_UpdateByPK";
                    cmd.Parameters.AddWithValue("@StudentID", model.StudentID);
                }

                cmd.Parameters.AddWithValue("@EnrollmentNo", model.EnrollmentNo);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo);
                cmd.Parameters.AddWithValue("@Address", model.Address ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Gender", model.Gender);
                cmd.Parameters.AddWithValue("@PlayingCricket", model.PlayingCricket);
                cmd.Parameters.AddWithValue("@Password", model.Password);
                cmd.Parameters.AddWithValue("@ConfirmPassword", model.ConfirmPassword);
                cmd.Parameters.AddWithValue("@Percentage12th", model.Percentage12th);
                cmd.Parameters.AddWithValue("@LiveInRajkot", model.LiveInRajkot);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("StudentList");
        }

        // Delete
        public IActionResult StudentDelete(int StudentID)
        {
            string connStr = _configuration.GetConnectionString("HMS_Tables");
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("PR_Student_DeleteByPK", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StudentID", StudentID);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("StudentList");
        }
    }
}
