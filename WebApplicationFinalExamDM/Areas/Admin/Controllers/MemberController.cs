using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebApplicationFinalExamDM.Contexts;
using WebApplicationFinalExamDM.Helpers;
using WebApplicationFinalExamDM.Models;
using WebApplicationFinalExamDM.ViewModels.MemberViewModels;

namespace WebApplicationFinalExamDM.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly string _folderPath;

        public MemberController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _folderPath = Path.Combine(_environment.WebRootPath, "images");
        }

        public async Task<IActionResult> Index()
        {
            var members = await _context.Members.Select(x => new MemberGetVM()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                ImagePath = x.ImagePath,
                PositionId = x.PositionId,
                PositionName = x.Position.Name
            }).ToListAsync();
            return View(members);
        }
        public async Task<IActionResult> Create()
        {
            await _SendPositionsWithViewBag();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MemberCreateVM vm)
        {
            if(!ModelState.IsValid)
            {
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            var positions = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);
            if(!positions)
            {
                ModelState.AddModelError("PositionId", "This position do not exist");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            if(!vm.Image.CheckType("image"))
            {
                ModelState.AddModelError("image", "Here only image type of files accepted");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            if (!vm.Image.CheckSize(2))
            {
                ModelState.AddModelError("image", "Here only max size of 2mb of image type of files accepted");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            string uniqueImagePath = await vm.Image.FileUploadAsync(_folderPath);
            Member member = new()
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                PositionId = vm.PositionId,
                ImagePath = uniqueImagePath
            };
            await _context.Members.AddAsync(member);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if(member is null)
            {
                return NotFound();
            }
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            string deletedImagePath = Path.Combine(_folderPath, member.ImagePath);
            ExtensionMethods.DeleteFile(deletedImagePath);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member is null)
            {
                return NotFound();
            }
            MemberUpdateVM vm = new()
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                PositionId = member.PositionId
            };
            await _SendPositionsWithViewBag();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(MemberUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            var positions = await _context.Positions.AnyAsync(x => x.Id == vm.PositionId);
            if (!positions)
            {
                ModelState.AddModelError("PositionId", "This position do not exist");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            if (!vm.Image?.CheckType("image")??false)
            {
                ModelState.AddModelError("image", "Here only image type of files accepted");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            if (!vm.Image?.CheckSize(2)??false)
            {
                ModelState.AddModelError("image", "Here only max size of 2mb of image type of files accepted");
                await _SendPositionsWithViewBag();
                return View(vm);
            }
            var isexistMember = await _context.Members.FindAsync(vm.Id);
            if(isexistMember is null)
            {
                return BadRequest();
            }
            isexistMember.FirstName = vm.FirstName;
            isexistMember.LastName = vm.LastName;
            isexistMember.PositionId = vm.PositionId;
            if(vm.Image is { })
            {
                string newImagePath = await vm.Image.FileUploadAsync(_folderPath);
                string oldImagePath = Path.Combine(_folderPath, isexistMember.ImagePath);
                ExtensionMethods.DeleteFile(oldImagePath);
                isexistMember.ImagePath = newImagePath;
            }
            _context.Members.Update(isexistMember);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private async Task _SendPositionsWithViewBag()
        {
            var positions = await _context.Positions.Select(x => new SelectListItem()
            {
                Value = x.Id.ToString(),
                Text = x.Name
            }).ToListAsync();
            ViewBag.Positions = positions;
        }
    }
}
