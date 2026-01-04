using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region EmployeeList
        public IActionResult EmployeeList()
        {
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
            connection.Open();

            SqlCommand command = new SqlCommand("PR_Employee_SelectAll", connection);
            command.CommandType = CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);

            connection.Close();
            return View(table);
        }
        #endregion

        #region EmployeeAddEdit [GET]
        [HttpGet]
        public IActionResult EmployeeAddEdit(int? EmployeeId)
        {
            if (EmployeeId != null && EmployeeId > 0)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Employee_SelectByPK", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    EmployeeModel model = new EmployeeModel
                    {
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"]?.ToString(),
                        DateOfBirth = reader["DateOfBirth"] as DateTime?,
                        Gender = reader["Gender"]?.ToString(),
                        HireDate = Convert.ToDateTime(reader["HireDate"]),
                        JobTitle = reader["JobTitle"]?.ToString(),
                        Department = reader["Department"]?.ToString(),
                        Salary = reader["Salary"] != DBNull.Value ? Convert.ToDecimal(reader["Salary"]) : (decimal?)null,
                        IsActive = Convert.ToBoolean(reader["IsActive"]),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"]),
                        UpdatedAt = reader["UpdatedAt"] as DateTime?
                    };
                    connection.Close();
                    return View(model);
                }
                connection.Close();
            }

            return View(new EmployeeModel());
        }
        #endregion

        #region EmployeeAddEdit [POST]
        [HttpPost]
        public IActionResult EmployeeAddEdit(EmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command;
                if (model.EmployeeId > 0)
                {
                    command = new SqlCommand("PR_Employee_UpdateByPK", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeId", model.EmployeeId);
                }
                else
                {
                    command = new SqlCommand("PR_Employee_Insert", connection);
                    command.CommandType = CommandType.StoredProcedure;
                }

                command.Parameters.AddWithValue("@FirstName", model.FirstName);
                command.Parameters.AddWithValue("@LastName", model.LastName);
                command.Parameters.AddWithValue("@Email", model.Email);
                command.Parameters.AddWithValue("@PhoneNumber", (object?)model.PhoneNumber ?? DBNull.Value);
                command.Parameters.AddWithValue("@DateOfBirth", (object?)model.DateOfBirth ?? DBNull.Value);
                command.Parameters.AddWithValue("@Gender", (object?)model.Gender ?? DBNull.Value);
                command.Parameters.AddWithValue("@HireDate", model.HireDate);
                command.Parameters.AddWithValue("@JobTitle", (object?)model.JobTitle ?? DBNull.Value);
                command.Parameters.AddWithValue("@Department", (object?)model.Department ?? DBNull.Value);
                command.Parameters.AddWithValue("@Salary", (object?)model.Salary ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsActive", model.IsActive);
                command.Parameters.AddWithValue("@CreatedAt", model.CreatedAt == DateTime.MinValue ? DateTime.Now : model.CreatedAt);
                command.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "Employee saved successfully.";
                return RedirectToAction("EmployeeList");
            }

            return View(model);
        }
        #endregion

        #region EmployeeDelete
        public IActionResult EmployeeDelete(int EmployeeId)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("HMS_Tables"));
                connection.Open();

                SqlCommand command = new SqlCommand("PR_Employee_DeleteByPK", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@EmployeeId", EmployeeId);

                command.ExecuteNonQuery();
                connection.Close();

                TempData["SuccessMessage"] = "Employee deleted successfully.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Error deleting employee.";
            }

            return RedirectToAction("EmployeeList");
        }
        #endregion
    }
}
