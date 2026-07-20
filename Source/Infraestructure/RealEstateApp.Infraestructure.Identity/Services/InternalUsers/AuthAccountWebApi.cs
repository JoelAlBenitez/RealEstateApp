using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Contracts.Users.InternalUsers;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace RealEstateApp.Infraestructure.Identity.Services.InternalUsers
{
    public sealed class AuthAccountWebApi : IAutheAccountWebApi
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IGenerateTokens _generateTokens;

        public AuthAccountWebApi(
            UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
            IGenerateTokens generateTokens
            
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateTokens = generateTokens;
        }

        public async Task<LoginApiDtoResponse> LoginAsync(LoginDto loginDto)
        {
            var response = new LoginApiDtoResponse
            {
                Errors = new List<string>(),
                HasError = false,
                Roles = new List<string>(),
                Token = null!,
                UserName = "",
            };
            var existUserByUserName = await _userManager.FindByNameAsync(loginDto.EmailOrNameUser);
            JwtSecurityToken jwt = null!;

            if (existUserByUserName != null) {
                var validate = await ValidateLogin(response, existUserByUserName, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUserByUserName);
                jwt = await _generateTokens.GenerateJwtToken(existUserByUserName);
                response.Token  = new JwtSecurityTokenHandler().WriteToken(jwt);
                response.UserName = existUserByUserName.UserName!;
                response.Roles = roles.ToList();
                response.Expiration = jwt.ValidTo;
                return response;
            }
            var existUserByEmail = await _userManager.FindByEmailAsync(loginDto.EmailOrNameUser);
            if (existUserByEmail != null) {
                var validate = await ValidateLogin(response, existUserByEmail, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUserByEmail);
                jwt = await _generateTokens.GenerateJwtToken(existUserByEmail);
                response.Token = new JwtSecurityTokenHandler().WriteToken(jwt);
                response.UserName = existUserByEmail.Email!;
                response.Roles = roles.ToList();
                response.Expiration = jwt.ValidTo;
                return response;

            }
            response.HasError = true;
            response.Errors.Add("Los datos de acceso son inválidos.");
            return response;
        }

        #region private methods

        private async Task<LoginApiDtoResponse> ValidateLogin(LoginApiDtoResponse response,
          AppUsers user, LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.EmailOrNameUser))
            {
                response.HasError = true;
                response.Errors.Add("Los datos de acceso son inválidos.");
                return response;
            }
            if (!user.EmailConfirmed && !user.IsActive)
            {
                response.HasError = true;
                response.Errors.Add("El usuario se encuentra inactivo y no puede autenticarse.");
                return response;
            }
            var rolesUser = await _userManager.GetRolesAsync(user);
            if (rolesUser.Contains(Roles.Agente.ToString())
                || rolesUser.Contains(Roles.Cliente.ToString())
                )
            {
                response.HasError = true;
                response.Forbidden = true;
                response.Errors.Add("El usuario no tiene permisos para acceder a esta API.");
                return response;
            }

            var verifyUser = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);
            if (!verifyUser.Succeeded)
            {
                response.HasError = true;
                if (verifyUser.IsLockedOut)
                {
                    response.Errors.Add("Su cuenta se encuentra bloqueada por múltiples intentos fallidos. Intente nuevamente en 15 minutos o contacte con un administrador.");
                    return response;
                }
                else
                {
                    response.Errors.Add("Los datos de acceso son inválidos.");
                    return response;
                }
            }

            return response;
        }


        #endregion
    }
}
