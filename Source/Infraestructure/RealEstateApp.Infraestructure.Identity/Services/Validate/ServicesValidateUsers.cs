using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using RealEstateApp.Core.Application.Contracts.Users.Validation;
using RealEstateApp.Core.Application.DTOs.Users.Auth.Session;
using RealEstateApp.Core.Application.DTOs.Users.Operational;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using System.Text.RegularExpressions;
namespace RealEstateApp.Infraestructure.Identity.Services.Validate
{
    public sealed class ServicesValidateUsers
        : IServicesValidateUsers
    {


        private readonly UserManager<AppUsers> _userManager;
        public const string EmailRegex =
            @"^(?=.{1,254}$)(?=.{1,64}@)[A-Za-z0-9]+(?:[._%+-][A-Za-z0-9]+)*@[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*(?:\.[A-Za-z0-9]+(?:-[A-Za-z0-9]+)*)+$";
        private readonly IUserSession _userSession;

        public ServicesValidateUsers(
            UserManager<AppUsers> userManager,
            IUserSession userSession
            )
        {
            _userManager = userManager;
            _userSession = userSession;
        }

        #region methods externals users
        public async Task<UserResponseDto> CreateExternalValidateUserAsync
            (RegisterExternalUsersDto externalUsersDto,
            UserResponseDto response
            )
        {

            #region validate fields
            if (string.IsNullOrWhiteSpace(externalUsersDto.Email) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.PhoneNumber) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.NameUser) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.Name) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.LastName) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.Password) ||
                 string.IsNullOrWhiteSpace(externalUsersDto.ConfirmPassword)

                 )
            {

                response.HasError = true;
                response.Errors.Add("Debe completar todos los campos requeridos.");
                return response;
            }

            if (string.IsNullOrWhiteSpace(externalUsersDto.ProfileImg))
                response.Errors.Add("El archivo seleccionado no tiene un formato de imagen válido.");

            if (externalUsersDto.TypeUser != (int)Roles.Agente
                && externalUsersDto.TypeUser != (int)Roles.Cliente)
                response.Errors.Add("Debe indicar un tipo de usuario válido.");

            if (!Regex.IsMatch(externalUsersDto.PhoneNumber, "^(809|829|849)-\\d{3}-\\d{4}$"))
                response.Errors.Add("Debe ingresar un número de teléfono válido de República Dominicana.");
            if (!Regex.IsMatch(externalUsersDto.Email, EmailRegex))
                response.Errors.Add("Debe ingresar un correo electrónico válido.");
            if (externalUsersDto.Password != externalUsersDto.ConfirmPassword)
                response.Errors.Add("La contraseña y la confirmación de contraseña no coinciden.");

            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
            #endregion


            #region validate exits user

            var exitsEmail = await _userManager.FindByEmailAsync(externalUsersDto.Email);
            if (exitsEmail != null)
            {
                response.HasError = true;
                response.Errors.Add("Ya existe un usuario registrado con este correo electrónico.");
                return response;
            }
            var exitsByUserName = await _userManager.FindByNameAsync(externalUsersDto.NameUser);
            if (exitsByUserName != null)
            {
                response.HasError = true;
                response.Errors.Add("Ya existe un usuario registrado con este nombre de usuario.");
                return response;
            }


            #endregion

            return response;

        }
        public async Task<EditResponseDto> UpdateExternalValidateUserAsync
           (EditAgentUserDto editAgentUserDto, EditResponseDto response)
        {

            #region validate fields
            if (string.IsNullOrWhiteSpace(editAgentUserDto.Name) ||
               string.IsNullOrWhiteSpace(editAgentUserDto.LastName) ||
            string.IsNullOrWhiteSpace(editAgentUserDto.PhoneNumber) ||
            string.IsNullOrWhiteSpace(editAgentUserDto.ProfileImg))
            {
                response.HasError = true;
                response.Errors.Add("Datos inválidos. Complete los datos correctamente.");
                return response;
            }
            if (!Regex.IsMatch(editAgentUserDto.PhoneNumber, "^(809|829|849)-\\d{3}-\\d{4}$"))
                response.Errors.Add("Debe ingresar un número de teléfono válido de República Dominicana.");

            if (editAgentUserDto.ChangePorfileImg && string.IsNullOrWhiteSpace(editAgentUserDto.ProfileImg))
                response.Errors.Add("El archivo seleccionado no tiene un formato de imagen válido.");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
            #endregion

            #region validate user
            var existUser = await _userManager.FindByIdAsync(editAgentUserDto.Id);
            if (existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Su usuario presenta problemas. Intente nuevamente más tarde.");
                return response;
            }
            var users = await _userManager.GetRolesAsync(existUser);
            if (!users.Contains(Roles.Agente.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("No posee los privilegios suficientes para ejecutar esta operación.");
                return response;
            }

            #endregion

            return response;

        }

        #endregion


        #region internal users methods

        public async Task<UserResponseDto> CreateInternalValidateUserAsync
            (RegisterInternalUsersDto internalUsersDto,
            UserResponseDto response)
        {
            if (internalUsersDto.TypeUser != (int)Roles.Desarrollador
                && internalUsersDto.TypeUser != (int)Roles.Administrador)
            {
                response.HasError = true;
                response.Errors.Add("El rol especificado para este usuario no es válido.");
                return response;
            }

            #region validate fields
            if (string.IsNullOrWhiteSpace(internalUsersDto.Email) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.IDCard) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.NameUser) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.Name) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.LastName) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.Password) ||
                 string.IsNullOrWhiteSpace(internalUsersDto.ConfirmPassword)

                 )
            {

                response.HasError = true;
                response.Errors.Add("Debe completar todos los campos requeridos.");
                return response;
            }
            if (internalUsersDto.Password != internalUsersDto.ConfirmPassword)
                response.Errors.Add("La contraseña y la confirmación de contraseña no coinciden.");
            if (!Regex.IsMatch(internalUsersDto.IDCard, @"^\d{11}$"))
                response.Errors.Add("La cédula ingresada no tiene un formato válido.");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }


            #endregion


            #region validate existencias
            var existUserName = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.UserName == internalUsersDto.NameUser);
            if (existUserName)
            {
                response.HasError = true;
                response.Errors.Add("El nombre de usuario ingresado no se encuentra disponible.");
                return response;
            }

            var existEmail = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == internalUsersDto.Email);
            if (existEmail) {

                response.HasError = true;
                response.Errors.Add("Ya existe un usuario registrado con este correo electrónico.");
                return response;
            }
            var existIdCard = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.IDCard == internalUsersDto.IDCard);
            if (existIdCard)
            {
                response.HasError = true;
                response.Errors.Add("La cédula ingresada ya se encuentra asociada a una cuenta.");
                return response;
            }

            #endregion


            return response;
        }


        public async Task<EditResponseDto> UpdateInternalValidateUserAsync
            (EditInternalUserDto editInternalUserDto, EditResponseDto response)
        {

            if (editInternalUserDto.Id == _userSession.GetIdCurrentUser())
            {
                response.HasError = true;
                response.Errors.Add("No puede editar su propio usuario desde este mantenimiento.");
                return response;
            }
            if (string.IsNullOrWhiteSpace(editInternalUserDto.Email)
                || string.IsNullOrWhiteSpace(editInternalUserDto.UserName)
                || string.IsNullOrWhiteSpace(editInternalUserDto.Name)
                || string.IsNullOrWhiteSpace(editInternalUserDto.LastName)
                || string.IsNullOrWhiteSpace(editInternalUserDto.IdCard)
                )
            {
                response.HasError = true;
                response.Errors.Add("Debe completar todos los campos requeridos.");
                return response;
            }

            var existUser = await _userManager.FindByIdAsync(editInternalUserDto.Id);
            if(existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("El usuario especificado no fue encontrado. Intente de nuevo.");
                return response;
            }
            var roles = await _userManager.GetRolesAsync(existUser);
            if(!roles.Contains(Roles.Desarrollador.ToString()) &&
                !roles.Contains(Roles.Administrador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("El usuario especificado no puede ser modificado desde este mantenimiento.");
                return response;
            }
            var existOtherUserByEmail = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.Email == editInternalUserDto.Email && u.Id != editInternalUserDto.Id);
            var existOtherUsersByUserName = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.UserName == editInternalUserDto.UserName && u.Id != editInternalUserDto.Id);
            var existOtherUsersByIdCard = await _userManager.Users.AsNoTracking()
                .AnyAsync(u => u.IDCard == editInternalUserDto.IdCard && u.Id != editInternalUserDto.Id);
            if (existOtherUserByEmail)
                response.Errors.Add("Ya existe un usuario registrado con este correo electrónico.");
            if (editInternalUserDto.NewPassword != editInternalUserDto.ConfirmNewPassword)
                response.Errors.Add("La contraseña y la confirmación de contraseña no coinciden.");
            if (existOtherUsersByUserName)
                response.Errors.Add("Ya existe un usuario registrado con este nombre de usuario.");
            if (existOtherUsersByIdCard)
                response.Errors.Add("La cédula ingresada ya se encuentra asociada a una cuenta.");
            if (response.Errors.Any())
            {
                response.HasError = true;
                return response;
            }
           


            return response;
        }

        #endregion

    }
}
