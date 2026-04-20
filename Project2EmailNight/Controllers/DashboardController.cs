using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project2EmailNight.Context;
using Project2EmailNight.Entities;
using System.Linq;

namespace Project2EmailNight.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailContext _context;

        public DashboardController(UserManager<AppUser> userManager, EmailContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            ViewBag.UserName = user.Name + " " + user.Surname;
            ViewBag.UserStatus = "Aktif";
            ViewBag.UserEmail = user.Email;

            var today = DateTime.Today;
            var weekStart = today.AddDays(-7);
            var monthStart = today.AddMonths(-1);
            var userMail = user.Email;

            ViewBag.DailyMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.SendDate.Date == today && x.IsDraft == false).Count();
            ViewBag.WeeklyMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.SendDate >= weekStart && x.IsDraft == false).Count();
            ViewBag.MonthlyMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.SendDate >= monthStart && x.IsDraft == false).Count();
            ViewBag.TotalMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.IsDraft == false).Count();
            ViewBag.ReadMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.IsRead == false).Count();
            ViewBag.UnreadMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.IsRead == false).Count();
            ViewBag.StarredMessage = _context.Messages.Where(x => x.ReceiverEmail == userMail && x.IsStarred == false).Count();
            ViewBag.DraftMessage = _context.Messages.Where(x => x.SenderEmail == userMail && x.IsDraft).Count();


            return View();
        }
    }
}