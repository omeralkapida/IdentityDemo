﻿using IdentityDemo.Dtos;
using IdentityDemo.Models;
using IdentityDemoSysteam.Dtos;
using IdentityDemoSysteam.Infrastructure.GoogleEmail;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityDemo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly IEmailService _emailService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            var errors = new List<string>();

            if (!ModelState.IsValid)
            {
                errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
            }

            // Kullanıcı adı var mı?
            var existingUserByName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUserByName != null)
            {
                errors.Add("Bu kullanıcı adı zaten alınmış.");
            }
            else
            {
                // E-posta var mı?
                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                    errors.Add("Bu e-posta adresi zaten kullanılıyor.");
            }

            if (errors.Any())
                return BadRequest(errors);

            // Yeni kullanıcı oluştur
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Ok("Kayıt başarılı");
            }

            // Identity framework'ten gelen hatalar
            var identityErrors = result.Errors.Select(e => e.Description).ToList();
            return BadRequest(identityErrors);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(errors);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return BadRequest(new List<string> { "Kullanıcı bulunamadı." });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest(new List<string> { "Şifre yanlış veya hesap geçersiz." });
            }

            // Claim bilgileri
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserName", user.UserName ?? ""),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("FullName", user.FullName ?? "")
            };

            var identity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Cookie oturumu başlat (non-persistent: sayfa kapanınca biter)
            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            return Json(new { redirectUrl = Url.Action("Index", "Home") });
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ViewBag.Message = "Bu e-posta sistemde kayıtlı değil.";
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, token = token }, protocol: Request.Scheme);

            Console.WriteLine("Şifre sıfırlama linki: " + callbackUrl);
            await _emailService.SendPasswordResetEmailAsync(user.Email!, callbackUrl!);

            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return BadRequest("Geçersiz bağlantı.");
            }

            var model = new ResetPasswordViewModel
            {
                UserId = userId,
                Token = token
            };

            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var errors = new List<string>();

            if (!ModelState.IsValid)
            {
                errors.AddRange(ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
            }

            if (errors.Any())
                return BadRequest(errors);

            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                errors.Add("Kullanıcı bulunamadı.");
                return BadRequest(errors);
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
                return Json(new { redirectUrl = Url.Action("Login", "Account") });

            errors.AddRange(result.Errors.Select(e => e.Description));
            return BadRequest(errors);
        }
    }
}
