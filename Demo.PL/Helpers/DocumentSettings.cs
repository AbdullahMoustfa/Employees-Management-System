using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.PL.Helpers
{
    public static class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            //// The Full Paht D:\MVC Demos\ProjectsName\Demo.PL\wwwroot\files\

            // 1. Get Located File Path 
            // 2. Get File Name and Make it UNIQUE
            // 3. Get File Path 
            // 4. Save File as Streams [Data Per Time]

            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            string filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            return fileName;


            //// 1. Locate Folder Path												ectures, courses,user)
            //var folderpath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/Files/", folderName);
            //// 2. Set Unique Names for Files (  new id  + fileName)

            //var filename = $"{Guid.NewGuid()}-{Path.GetFileName(file.FileName)}";

            //// 3. get file path

            //var filepath = Path.Combine(folderpath, filename);

            //// 4. upload file on server

            //using var filestream = new FileStream(filepath, FileMode.Create);

            //file.CopyTo(filestream);

            //return filename;



        }

        // Delete FileName
        public static void DeleteFile(string fileName, string folderName)
        {
            
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "/wwwroot/Files", fileName);  
            if(File.Exists(filePath))
               File.Delete(fileName);  
        }
    }
}
