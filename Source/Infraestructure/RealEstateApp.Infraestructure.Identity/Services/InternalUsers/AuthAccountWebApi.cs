using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Services.InternalUsers
{
    public sealed class AuthAccountWebApi : IAutheAccountWebApi
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;

        public AuthAccountWebApi(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
        {
            var response = new UserResponseDto
            {
                Errors = new List<string>(),
                HasError = false,
                Roles = new List<string>()
            };



            return response;
        }

        #region private methods

        private async Task<UserResponseDto> ValidateLogin(UserResponseDto response,
          AppUsers user, LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.EmailOrNameUser))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Los datos de acceso son inválidos.");
                return response;
            }
            if (!user.EmailConfirmed && !user.IsActive)
            {
                response.HasError = true;
                response.Errors.Add("Su cuenta se encuentra inactiva." +
                    "Pongase en contacto con un administrador.");
                return response;
            }
            var rolesUser = await _userManager.GetRolesAsync(user);
            if (rolesUser.Contains(Roles.Desarrollador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("El usuario no tiene un rol válido asignado." +
                    " Póngase en contacto con un administrador.");
                return response;
            }

            var verifyUser = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);
            if (!verifyUser.Succeeded)
            {
                response.HasError = true;
                if (verifyUser.IsLockedOut)
                {
                    response.Errors.Add("Su cuenta se encuentra bloqueada por multiples intentos repetidos fallidos." +
                        " Favor intente nuevamente dentro de 15 minutos o contacte con un administrador.");
                    return response;
                }
                else
                {
                    response.Errors.Add("El nombre de usuario o la contraseña son incorrectos.");
                    return response;
                }
            }

            return response;
        }

        #endregion
    }
}
