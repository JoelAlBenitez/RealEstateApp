using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Contracts.FileManager;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
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

       public AccountController(
           IAuthProcesssAccountWebApp authProcesssAccountWebApp,
           IMapper mapper,
           IFileManager fileManager
           )
        {
            _authProcesssAccountWebApp = authProcesssAccountWebApp;
            _mapper = mapper;
            _fileManager = fileManager;
        }

        #region Load Views
        public IActionResult Login(bool expire = false)
        {
            #region redirection home
            if (User.IsInRole(Roles.Agente.ToString()))
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            else if(User.IsInRole(Roles.Cliente.ToString()))
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            else if (User.IsInRole(Roles.Administrador.ToString()))
                return RedirectToRoute(new {controller = "Home", action = "Index"});
            #endregion
            if (expire)
                TempData["Message"] = "Su sesión finalizó por inactividad. Inicie sesión nuevamente para continuar.";

            return View(new LoginUserViewModel
            {
                EmailOrNameUser = "",
                Password = ""
            });
        }

        public IActionResult ResendConfirmAccount()
        {
            return View(
                new ResendActivationEmailViewModel { UserName = ""}
            );
        }
        public IActionResult ResetPassword(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
            {
                return RedirectToAction(nameof(Login));
            }
            return View( new ResetPasswordViewModel
            {
                ConfirmNewPassword = "",
                NewPassword = "",
                Id  = userId,
                Token = token
            });
        }
        public IActionResult RegisterExternalUser()
        {
            return View(
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
            return View(new ForgoutPasswordViewModel { UserName = "" });
        }

        #endregion

        #region Post Views


        #endregion
    }
}
