using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplicationFinalExamDM.Contexts;
using WebApplicationFinalExamDM.Models;
using WebApplicationFinalExamDM.ViewModels.PositionViewModels;

namespace WebApplicationFinalExamDM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PositionController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var positions = await _context.Positions.Select(x => new PositionGetVM()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();
            return View(positions);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(PositionCreateVM vm)
        {
            if(!ModelState.IsValid)
            {
                return View(vm);
            }
            Position position = new()
            {
                Name = vm.Name
            };
            await _context.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if(position is null)
            {
                return NotFound();
            }
            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position is null)
            {
                return NotFound();
            }
            PositionUpdateVM vm = new()
            {
                Id = position.Id,
                Name = position.Name
            };
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(PositionUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var isexistPosition = await _context.Positions.FindAsync(vm.Id);
            if(isexistPosition is null)
            {
                return BadRequest();
            }
            isexistPosition.Name = vm.Name;
            _context.Positions.Update(isexistPosition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
