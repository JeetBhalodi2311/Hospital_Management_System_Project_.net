using Hospital_Management_System.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Data;
using System.Data.SqlClient;
using Praticse.Helpers;


namespace Hospital_Management_System.Controllers
{
    public class ImageController : Controller
    {
        [HttpGet]
        public IActionResult ImageAddEdit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImageAddEdit(ImageModel img)
        {
            ViewBag.ImageName = img.ImageName;

            string filePath = "";
            try
            {
                filePath = ImageHelper.SaveImage(img.Image, "Profile");
                ViewBag.ImagePath = filePath; 
            }
            catch (System.Exception)
            {
                ViewBag.ImagePath = "";
                Console.WriteLine("File not provided or error occurred.");
            }

            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
