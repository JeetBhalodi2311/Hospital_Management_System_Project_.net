//namespace Praticse.Helpers;

//using System;
//using System.IO;
//public class ImageHelper
//{
//    public static string SaveImage(IFormFile imageFile, string dir)
//    {
//        string finalDirPath = $"wwwroot/{dir}";
//        if (imageFile == null || imageFile.Length == 0)
//        {
//            throw new Exception();
//        }

//        if (!Directory.Exists(finalDirPath))
//        {
//            Directory.CreateDirectory(finalDirPath);
//        }
//        //extract extension from file
//        string fileExtension = Path.GetExtension(imageFile.FileName);

//        //genrate unique file name
//        string uniqueNameForFile = $"{Guid.NewGuid()}.{fileExtension}";

//        //get full path which we will store in db ( we dont need to store from wwwroot)
//        string fullPathToStoreInDB = $"{dir}/{uniqueNameForFile}";

//        //get path where we store image means wwwroot
//        string fullPathToWrite = $"{finalDirPath}/{uniqueNameForFile}";


//        // use stream to manipulate or save image in disk
//        FileStream stream = new FileStream(fullPathToWrite, FileMode.CreateNew);
//        imageFile.CopyTo(stream);
//        stream.Close();

//        //retrurn path which we will store in db
//        return fullPathToStoreInDB;
//    }
//}


using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Praticse.Helpers
{
    public class ImageHelper
    {
        public static string SaveImage(IFormFile imageFile, string dir)
        {
            string finalDirPath = $"wwwroot/{dir}";
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new Exception("No file provided.");
            }

            if (!Directory.Exists(finalDirPath))
            {
                Directory.CreateDirectory(finalDirPath);
            }

            // Extract extension from file (remove leading dot to avoid ..jpg)
            string fileExtension = Path.GetExtension(imageFile.FileName).TrimStart('.');

            // Generate unique file name
            string uniqueNameForFile = $"{Guid.NewGuid()}.{fileExtension}";

            // Path to store in DB (relative to wwwroot)
            string fullPathToStoreInDB = $"{dir}/{uniqueNameForFile}";

            // Actual path on disk
            string fullPathToWrite = Path.Combine(finalDirPath, uniqueNameForFile);

            // Save image
            using (var stream = new FileStream(fullPathToWrite, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return fullPathToStoreInDB;
        }
    }
}
