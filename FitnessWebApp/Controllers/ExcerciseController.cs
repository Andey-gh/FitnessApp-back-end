using FitnessWebApp.Domain;
using FitnessWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace FitnessWebApp.Controllers
{
    [Authorize]
    [Route("/api")]
    [ApiController]
    public class ExcerciseController : Controller
    {
        
        private AppDbContext _context;

        public ExcerciseController(AppDbContext context)
        {

            _context = context;
        }

        [HttpPost]
        [Route("Excercises")]
        public async Task<ActionResult<Excercise>> Post(Excercise excercise)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(excercise);
                await _context.SaveChangesAsync();
                return Json(excercise);
            }
            return UnprocessableEntity();

        }

        [HttpGet]
        [Route("Excercises")]
        public async Task<ActionResult<ICollection<Excercise>>> GetExcercises()
        {

            return await _context.Excercises.ToListAsync();
        }
        [HttpGet("Excercises/{id}")]

        public async Task<ActionResult<Excercise>> GetExcercise(int id)
        {
            if (ModelState.IsValid)
            {
                var excercise_id = await _context.Excercises.FindAsync(id);

                if (excercise_id == null)
                {
                    return NotFound();
                }

                return Json(excercise_id);

            }
            return UnprocessableEntity();

        }

        [HttpDelete("Excercises/{id}")]
        public async Task<ActionResult<Excercise>> DeleteExcercise(int id)
        {
            if (ModelState.IsValid)
            {
                var excercise = await _context.Excercises.FindAsync(id);
                if (excercise == null)
                {
                    return NotFound();
                }

                _context.Excercises.Remove(excercise);
                await _context.SaveChangesAsync();

                return Ok("Excercise was deleted");
            }
            return UnprocessableEntity();

        }
    }
}
