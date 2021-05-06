using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Controllers
{
   
    [Route("/api")] 
    [ApiController]
    public class ImageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ImageController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Image")]

        public async Task<IActionResult> AddImage([FromForm(Name = "file")] IFormFile ImageFile)
        {
            string wwwRootPath = _environment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string extension = Path.GetExtension(ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image_" + fileName);

            using (var fileStream = new FileStream(path, FileMode.Create))
            {

                await ImageFile.CopyToAsync(fileStream);
            }

            return Ok();
        }
    }
}
