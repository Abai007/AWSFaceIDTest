using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Mvc;

namespace FaceID.Controllers
{
    public class CameraController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        public CameraController(IWebHostEnvironment environment) =>
            _environment = environment;
        public IActionResult Capture()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CapturePhoto(string name)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                if (files != null)
                {
                   
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var fileName = file.FileName;
                            var myUniqueFileName = Convert.ToString(Guid.NewGuid());
                            var fileExtension = Path.GetExtension(fileName);
                            var newFileName = string.Concat(myUniqueFileName, fileExtension);
                            var filePath = Path.Combine(_environment.WebRootPath, "CameraPhotos") + $@"\{newFileName}";
                            if (!string.IsNullOrEmpty(filePath))
                                StoreInFolder(file, filePath);

                            var imageBytes = System.IO.File.ReadAllBytes(filePath);
                            if (imageBytes != null)
                                StoreInDataBase(imageBytes);
                        } 

                    }
                    
                    return Json(true);
                }
                
                return Json(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }

        private void StoreInDataBase(byte[] imageBytes)
        {
            //Надо написать сохранение в БД
        }
    }
}
