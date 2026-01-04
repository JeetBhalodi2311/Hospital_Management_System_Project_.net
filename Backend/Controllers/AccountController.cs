using BCrypt.Net;
using Hospital_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

public class AccountController : Controller
{
    private readonly IConfiguration _config;

    public AccountController(IConfiguration config)
    {
        _config = config;
    }

    // GET: Login
    public IActionResult Login() => View();

    // GET: Register
    public IActionResult Register() => View();

    // POST: Register
    [HttpPost]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("HMS_Tables")))
        {
            conn.Open();
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

            SqlCommand cmd = new SqlCommand(
                "INSERT INTO Admin (FullName, Email, Password, Role) VALUES (@FullName, @Email, @Password, @Role)", conn);

            cmd.Parameters.AddWithValue("@FullName", model.FullName);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Password", hashedPassword);
            cmd.Parameters.AddWithValue("@Role", model.Role);

            cmd.ExecuteNonQuery();
        }

        TempData["Success"] = "Registration successful!";
        return RedirectToAction("Login");
    }

    // POST: Login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("HMS_Tables")))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE Email = @Email", conn);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                string storedHash = reader["Password"].ToString();
                string role = reader["Role"].ToString();
                string name = reader["FullName"].ToString();

                if (BCrypt.Net.BCrypt.Verify(model.Password, storedHash))
                {
                    HttpContext.Session.SetString("UserName", name);
                    HttpContext.Session.SetString("UserRole", role);

                    return RedirectToAction("Index", "Dashboard");
                }
            }
        }

        ModelState.AddModelError("", "Invalid user. Please enter a valid username or password.");
        return View(model);
    }

    // GET: Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}


//using BCrypt.Net;
//using Hospital_Management_System.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using System.Data.SqlClient;

//namespace Hospital_Management_System.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly IConfiguration _config;

//        public AccountController(IConfiguration config)
//        {
//            _config = config;
//        }

//        // GET: Login
//        public IActionResult Login() => View();

//        // GET: Register
//        public IActionResult Register() => View();

//        // POST: Register
//        [HttpPost]
//        public IActionResult Register(RegisterViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("HMS_Tables")))
//            {
//                conn.Open();
//                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);

//                SqlCommand cmd = new SqlCommand(
//                    "INSERT INTO Admin (FullName, Email, Password, Role) VALUES (@FullName, @Email, @Password, @Role)", conn);

//                cmd.Parameters.AddWithValue("@FullName", model.FullName);
//                cmd.Parameters.AddWithValue("@Email", model.Email);
//                cmd.Parameters.AddWithValue("@Password", hashedPassword);
//                cmd.Parameters.AddWithValue("@Role", model.Role);

//                cmd.ExecuteNonQuery();
//            }

//            TempData["Success"] = "Registration successful!";
//            return RedirectToAction("Login");
//        }

//        // POST: Login
//        [HttpPost]
//        public IActionResult Login(LoginViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("HMS_Tables")))
//            {
//                conn.Open();
//                SqlCommand cmd = new SqlCommand("SELECT * FROM Admin WHERE Email = @Email", conn);
//                cmd.Parameters.AddWithValue("@Email", model.Email);
//                SqlDataReader reader = cmd.ExecuteReader();

//                if (reader.Read())
//                {
//                    string storedHash = reader["Password"].ToString();
//                    string role = reader["Role"].ToString();
//                    string name = reader["FullName"].ToString();

//                    if (BCrypt.Net.BCrypt.Verify(model.Password, storedHash))
//                    {
//                        HttpContext.Session.SetString("UserName", name);
//                        HttpContext.Session.SetString("UserRole", role);

//                        return RedirectToAction("Index", "Dashboard");
//                    }
//                }
//            }

//            ModelState.AddModelError("", "Invalid user. Please enter a valid username or password.");
//            return View(model);
//        }

//        // GET: Logout
//        public IActionResult Logout()
//        {
//            HttpContext.Session.Clear(); // Clear session
//            return RedirectToAction("Login");
//        }
//    }
//}
