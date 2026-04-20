using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2EmailNight.Dtos;
using Project2EmailNight.Entities;

namespace Project2EmailNight.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public RegisterController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserRegisterDto userRegisterDto)
        {
            Random random = new Random();
            int confirmCode = random.Next(100000, 999999);

            AppUser appUser = new AppUser()
            {
                Name = userRegisterDto.Name,
                Surname = userRegisterDto.Surname,
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                ConfirmCode = confirmCode
            };
            var result=await _userManager.CreateAsync(appUser, userRegisterDto.Password);
            if (result.Succeeded)
            { 
                return RedirectToAction("VerifyCode", new { email = appUser.Email });
            }
            else 
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("CreateUser", userRegisterDto);
        }

        [HttpGet]
        public IActionResult VerifyCode(string email)
        {
            ViewBag.email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string email, int confirmCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return View();
            }

            if (user.ConfirmCode == confirmCode)
            {
                user.EmailConfirmed = true;
                user.ConfirmCode = 0;

                await _userManager.UpdateAsync(user);

                return RedirectToAction("UserLogin", "Login");
            }

            ModelState.AddModelError("", "Kod yanlış");
            return View();
        }
    }
}
