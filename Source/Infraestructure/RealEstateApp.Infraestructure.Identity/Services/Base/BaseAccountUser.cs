using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RealEstateApp.Core.Application.Contracts.Users.Base;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.DtoQueryUser.Base;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Infraestructure.Identity.Entities;

namespace RealEstateApp.Infraestructure.Identity.Services.Base
{
    public abstract class BaseAccountUser : IBaseAccountUser
    {
        protected readonly UserManager<AppUsers> _userManager;
        protected readonly SignInManager<AppUsers> _signInManager;
        protected readonly IUserSession _userSession;

        public BaseAccountUser(UserManager<AppUsers> userManager,
            SignInManager<AppUsers> signInManager,
             IUserSession userSession
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userSession = userSession;

        }

        #region method publics
        public virtual async Task<UserResponseDto> ChangeStateAsync(
            AlterStateUserDto alterStateUserDto
            )
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors = new List<string>()
            };

            var validate = await ValidateFieldsAndPermisions(response, alterStateUserDto.Id);
            if (validate != null && validate.HasError) return validate;

            var userChange = await _userManager.FindByIdAsync(alterStateUserDto.Id);
            if(userChange == null)
            {
                response.HasError = true;
                response.Errors.Add("Oops, al aparecer el usuario " +
                    "especificado no pudo ser encontrado. Intente de nuevo mas tarde");
            }
            userChange!.IsActive = alterStateUserDto.State;
            var changeResult = await _userManager.UpdateAsync(userChange);
            if (!changeResult.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("Oops, Ha ocurrido un error inesperado al procesar la solicitud, favor intente de nuevo mas tarde.");
                return response;
            }

            return response;
        }

        public virtual async Task<UserResponseDto> DeleteAsync(string IdUser)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors= new List<string>()
            };
            var validate = await ValidateFieldsAndPermisions(response, IdUser);
            if(validate != null && validate.HasError) return validate;

            var user = await _userManager.FindByIdAsync(IdUser);
            var invalidSessionUser = await _userManager.UpdateSecurityStampAsync(user!);
            var delete = await _userManager.DeleteAsync(user!);
            if (!delete.Succeeded && !invalidSessionUser.Succeeded)
            {
                response.HasError = true;
                response.Errors.Add("Oops, " +
                    "Ha ocurrido un error inesperado al procesar la solicitud, " +
                    "favor intente de nuevo mas tarde.");
                return response;
            }

            return response;

        }

        public virtual async Task<BaseGetUserDto> GetUserBaseById(string IdUser)
        {
            var result = await _userManager.FindByIdAsync(IdUser);
            if (result == null) return null!;
            return new BaseGetUserDto
            {
                Id = result.Id,
                Name = result.Name,
                LastName = result.LastName
            
            };
        }

        
        public virtual async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
        #endregion
        #region private methods
        private async Task<UserResponseDto> ValidateFieldsAndPermisions(UserResponseDto response, string Id)
        {

            var id = _userSession.GetIdCurrentUser();
            var existUser = await _userManager.FindByIdAsync(id);
            if (existUser == null || existUser.IsActive)
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer su usuario no se enecuentra habilitado, favor intente de nuevo.");
                return response;
            }
            var roles = _userSession.GetRolesCurrentUser();
            if (!roles.Contains("Administrador"))
            {
                response.HasError = true;
                response.Errors.Add("Su perfil no posee privilegios suficientes apra realizar esta accion");
                return response;
            }

            var userChange = await _userManager.FindByIdAsync(Id);
            if (userChange == null)
            {
                response.HasError = true;
                response.Errors.Add("Oops, al aparecer el usuario " +
                    "especificado no pudo ser encontrado. Intente de nuevo mas tarde");
            }
            return response;

        }



        #endregion
    }
}
