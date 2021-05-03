using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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
        private readonly IConfiguration _configuration;

        public ImageController(AppDbContext context, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Image")]
        public async Task<IActionResult> AddImage([FromForm(Name = "file")] IFormFile ImageFile)
        {
            string wwwRootPath = _environment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
            string extension = Path.GetExtension(ImageFile.FileName);
            if (extension.ToLower() != ".jpg" && extension.ToLower() != ".png") return StatusCode(400);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(@$"{wwwRootPath}Images/{fileName}");
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await ImageFile.CopyToAsync(fileStream);
            }
            string url = $"{_configuration.GetValue<string>("Domain")}/Images/{fileName}";
            var responce = new { Name = fileName, URL = url };
            return Json(responce);
        }
        /*
        [HttpPost]
        [AllowAnonymous]
        [Route("setImage")]
        public async Task<IActionResult> SetImage(FileModel file)
        {
            if (ModelState.IsValid) {
                if (file.Type != "Image") return StatusCode(400);
                switch (file.Table)
                {
                    case "Muscles": {
                       int id;
                       if(!int.TryParse(file.Id,out id)) return StatusCode(400);
                       var muscle = await _context.Muscles.FindAsync(id);
                    }; break;
                    case "MuscleGroup": ; break;
                    case "Excercises": ; break;
                    case "TrainingPlans": ; break;
                    default: return StatusCode(501);break;
                }
                return Ok();
            }
            return UnprocessableEntity();
        }
        */

        }
}
