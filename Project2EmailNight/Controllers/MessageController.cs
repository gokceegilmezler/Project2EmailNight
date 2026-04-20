using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Context;
using Project2EmailNight.Entities;
using Microsoft.EntityFrameworkCore;

namespace Project2EmailNight.Controllers
{
    public class MessageController : Controller
    {
        private readonly EmailContext _context;
        private readonly UserManager<AppUser> _userManager;
        public MessageController(EmailContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateMessage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateMessage(Message message)
        {
            message.SenderEmail = User.Identity.Name;
            message.SendDate = DateTime.Now;
            message.IsStatus = false;
            _context.Messages.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Sendbox");

        }

        public async Task<IActionResult> Inbox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userMail = user.Email;
            var values = await _context.Messages
                .Where(x => x.ReceiverEmail == userMail)
                .ToListAsync();
            ViewBag.unreadCount = values.Count(x => x.IsStatus == false);
            ViewBag.UserName = user.Name + " " + user.Surname;
            return View(values);
        }

        public async Task<IActionResult> SendBox()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userMail = User.Identity.Name;
            var values = await _context.Messages.Where(x => x.SenderEmail == userMail).ToListAsync(); 
            return View(values); 
        }

        public IActionResult Starred()
        {
            var values=_context.Messages.Where(x=>x.ReceiverEmail==User.Identity.Name && x.IsStarred==true).ToList();
            return View(values);
        }

        public IActionResult MessageDetail(int id)
        {
            var values = _context.Messages.Find(id);
            if (values == null)
            {
                return NotFound();
            }
            values.IsRead = true;
            _context.SaveChanges();
            return View(values);
        }

        public IActionResult DeleteMessage(int id)
        {
            var values=_context.Messages.Find(id);
            _context.Messages.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("Inbox");

        }
    }
}
