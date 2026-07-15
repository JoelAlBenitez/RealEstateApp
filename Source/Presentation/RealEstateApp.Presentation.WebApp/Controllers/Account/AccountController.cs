using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.ViewsModel.Users.Auth;
using RealEstateApp.Core.Application.ViewsModel.Users.Operational.ExternalUser;
using RealEstateApp.Core.Domain.Common.Enums;

namespace RealEstateApp.Presentation.WebApp.Controllers.Account
{
    public class AccountController : Controller
    {

        private readonly IAuthProcesssAccountWebApp _authProcesssAccountWebApp;
        private readonly IFileManager _fileManager;
        private readonly IMapper _mapper;
        private readonly IOperationalAccountWebApp _operationalAccountWebApp;

       public AccountController(
           IAuthProcesssAccountWebApp authProcesssAccountWebApp,
           IMapper mapper,
           IFileManager fileManager,
           IOperationalAccountWebApp operationalAccountWebApp

           )
        {
            _authProcesssAccountWebApp = authProcesssAccountWebApp;
            _mapper = mapper;
            _fileManager = fileManager;
           _operationalAccountWebApp = operationalAccountWebApp;
        }

        #region Load Views
        public IActionResult Login(bool expired = false)
        {
            #region redirection home
            if (User.IsInRole(Roles.Agente.ToString()))
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            else if(User.IsInRole(Roles.Cliente.ToString()))
                return RedirectToRoute(new {controller = "Customer", action = "Index"});
            else if (User.IsInRole(Roles.Administrador.ToString()))
                return RedirectToRoute(new {controller = "Admin", action = "Index"});
            #endregion
            if (expired)
                TempData["Message"] = "Su sesión finalizó por inactividad. Inicie sesión nuevamente para continuar.";

            return View(new LoginUserViewModel
            {
                EmailOrNameUser = "",
                Password = ""
            });
        }
        public IActionResult ResendConfirmAccount()
        {
            return View( "ResendActivationAccount",
                new ResendActivationEmailViewModel { UserName = ""}
            );
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(Login));
            }
            return View( "ResetPassword", new ResetPasswordViewModel
            {
                ConfirmNewPassword = "",
                NewPassword = "",
                Id  = userId,
                Token = token
            });
        }
        public IActionResult RegisterExternalUser()
        {
            return View( "RegisterExternalUser",
                new CreateExternalUserViewModel
                {
                    ConfirmPassword = "",
                    Name = "",
                    Email = "",
                    LastName = "",
                    NameUser = "",
                    Password = "",
                    PhoneNumber = "",
                    TypeUser =  0,
                    ProfileImg = null!
                });
        }
        public IActionResult ForgoutPassword()
        {
            return View("ForgoutPassword", new ForgoutPasswordViewModel { UserName = "" });
        }
        public async Task<IActionResult> ConfirmAccountEmail(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(Login));
            }
            string response = await _authProcesssAccountWebApp.ConfirmAccountByEmailAsync(token, userId);
            return View("ConfirmEmail", response);
        }

        //access deniged
        public async Task<IActionResult> AccessDeniged()
        {
            TempData["Message"] = "No tiene permisos para acceder a esta sección.";
            await _operationalAccountWebApp.SignOutAsync();
            if (User.IsInRole(Roles.Agente.ToString()))
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            else if (User.IsInRole(Roles.Cliente.ToString()))
                return RedirectToRoute(new { controller = "Customer", action = "Index" });
            else if (User.IsInRole(Roles.Administrador.ToString()))
                return RedirectToRoute(new { controller = "Admin", action = "Index" });
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Post Views

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _operationalAccountWebApp.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> RegisterExternalUser(CreateExternalUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Password = "";
                vm.ConfirmPassword = "";
                ModelState.AddModelError("", "Debe completar todos los campos requeridos.");
                return View(vm);
            }
            var file = await _fileManager.SaveAsync(vm.ProfileImg, "Users", Guid.NewGuid().ToString());
            var map = _mapper.Map<RegisterExternalUsersDto>(vm);
            map.Origin = Request?.Headers?.Origin.ToString() ?? string.Empty;
            map.ProfileImg = file;
            var result = await _operationalAccountWebApp.CreateExternalAsync(map);
            if(result != null && result.HasError)
            {
                vm.Password = "";
                vm.ConfirmPassword = "";
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("Error", error);
                }
                return View(vm);
            }
            TempData["Message"] = vm.TypeUser == (int)Roles.Cliente
                ? "Su cuenta ha sido creada correctamente. Revise su correo electrónico para activar su usuario."
                : "Su cuenta de agente ha sido creada correctamente. Un administrador debe activar su usuario antes de que pueda iniciar sesión.";
            return RedirectToAction(nameof(Login));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResendConfirmAccount(ResendActivationEmailViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Debe completar todos los campos requeridos.");
                return View(vm);
            }
            var map = _mapper.Map<ResendActivationEmailDto>(vm);
            map.Origin = Request?.Headers?.Origin.ToString() ?? string.Empty;
            var result = await _authProcesssAccountWebApp.ResendActivationEmailAsync(map);
            if (result != null && result.HasError)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                }
            }
            TempData["Message"] = "Si la cuenta existe y posee un correo válido, recibirá un correo de confirmación. Revise su bandeja de entrada.";
            return RedirectToAction(nameof(Login));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ForgoutPassword(ForgoutPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Debe completar todos los campos requeridos.");
                return View(vm);
            }
            var map = _mapper.Map<ForgoutPasswordDto>(vm);
            map.Origin = Request?.Headers?.Origin.ToString() ?? string.Empty;
            var result = await _authProcesssAccountWebApp.ForgoutPasswordAsync(map);
            if(result != null && result.HasError)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                    return View(vm);
                }
            }
            TempData["Message"] = "Si la cuenta existe y posee un correo válido, recibirá un correo de restablecimiento de contraseña. Revise su bandeja de entrada.";

            return RedirectToAction(nameof(Login));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
         if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Debe completar todos los campos requeridos.");
                return View(vm);
          }
         var map = _mapper.Map<ResetPasswordDto>(vm);
         var result = await _authProcesssAccountWebApp.ResetPasswordAsync(map);
         if (result != null && result.HasError)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                    return View(vm);
                }
            }
            TempData["Message"] = "Su contraseña fue cambiada correctamente. Inicie sesión y continúe.";
            return RedirectToAction(nameof(Login));
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Password = "";
                ModelState.AddModelError("", "Debe ingresar su correo o nombre de usuario y contraseña.");
                return View(vm);
            }
            var dto = _mapper.Map<LoginDto>(vm);
            var result = await _authProcesssAccountWebApp.LoginUser(dto);
            if (result != null && result.HasError)
            {
                vm.Password = "";
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item);
                }
                return View(vm);
            }
            #region redirection home
            if (result!.Roles.Contains((Roles.Agente.ToString())))
                return RedirectToRoute(new { controller = "Agent", action = "Index" });
            else if (result!.Roles.Contains(Roles.Cliente.ToString()))
                return RedirectToRoute(new { controller = "Customer", action = "Index" });
            else if (result!.Roles.Contains(Roles.Administrador.ToString()))
                return RedirectToRoute(new { controller = "Admin", action = "Index" });
            vm.Password = "";
            ModelState.AddModelError("", "Ha ocurrido un error inesperado.");
            return View(vm);
            #endregion

        }
        #endregion
    }
 }
