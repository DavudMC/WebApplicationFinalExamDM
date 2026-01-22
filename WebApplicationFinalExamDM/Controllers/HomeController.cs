using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplicationFinalExamDM.Contexts;
using WebApplicationFinalExamDM.ViewModels.MemberViewModels;


namespace WebApplicationFinalExamDM.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> IndexAsync()
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
    }
}
