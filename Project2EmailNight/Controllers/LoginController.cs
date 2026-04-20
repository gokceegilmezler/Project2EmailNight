using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        
        [HttpGet]
        public IActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View(userLoginDto);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Lütfen email adresinizi doğrulayın");
                return View(userLoginDto);
            }

            var result = await _signInManager.PasswordSignInAsync(userLoginDto.Username, userLoginDto.Password, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                return View(userLoginDto);
            }

        }
    }
}
