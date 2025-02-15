﻿using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessWebApp.Managers
{
    public class ExcercisesManager:IExcercisesManager
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        public ExcercisesManager(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ActionResult> AddExcercise(Excercise excercise)
        {
            string extension = Path.GetExtension(excercise.Image.FileName);
            if (extension.ToLower() != ".jpg" && extension.ToLower() != ".png") return new StatusCodeResult(400);
            var guid =  Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", "Images", guid + ".jpg");
            if (excercise.Image != null)
            {
                var fileStream = new FileStream(filePath, FileMode.Create);
                excercise.Image.CopyTo(fileStream);
            }
            excercise.Photo = $"{ _configuration.GetValue<string>("Domain")}"+filePath.Remove(0, 7);
            await _context.AddAsync(excercise);
            await _context.SaveChangesAsync();
            return new OkResult();
            
        }
        public async Task<List<Excercise>> GetExcercises()
        {
            return await _context.Excercises.ToListAsync();
        }
        
        public async Task<ActionResult> DeleteExcercise(int id)
        {
            var excercise = await _context.Excercises.FindAsync(id);
            if (excercise == null)
            {
                return new NotFoundResult();
            }

            _context.Excercises.Remove(excercise);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<ActionResult<Excercise>> GetExcerciseById(int id)
        {
            var excercise_id = await _context.Excercises.FindAsync(id);

            if (excercise_id == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(excercise_id);
        }

        
    }
}
