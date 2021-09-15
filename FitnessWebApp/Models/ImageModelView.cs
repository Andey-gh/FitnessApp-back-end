using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Models
{
    public class ImageModelView
    {
        
        public IFormFile ImageFile { get; set; }
    }
}
