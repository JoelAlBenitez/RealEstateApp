using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using RealEstateApp.Core.Application.Contracts.EmailServices;
using RealEstateApp.Core.Application.Contracts.Users.ExternalUsers;
using RealEstateApp.Core.Application.DTOs.Message;
using RealEstateApp.Core.Application.DTOs.Users.Auth;
using RealEstateApp.Core.Application.DTOs.Users.Password;
using RealEstateApp.Core.Application.DTOs.Users.Response;
using RealEstateApp.Core.Domain.Common.Enums;
using RealEstateApp.Infraestructure.Identity.Entities;
using RealEstateApp.Infraestructure.Identity.Interfaces;
using System.Text;

namespace RealEstateApp.Infraestructure.Identity.Services.ExternalUsers
{
    public sealed class AuthServicesProcccessAccountWebApp : IAuthProcesssAccountWebApp
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly IGenerateTokens _generateTokens;
        private readonly IEmailService _emailServices;
            
        public AuthServicesProcccessAccountWebApp(UserManager<AppUsers
            > userManager, SignInManager<AppUsers> signInManager,
            IGenerateTokens generateTokens,
            IEmailService emailService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _generateTokens = generateTokens;
            _emailServices = emailService;
        }

        #region process account
        public async Task<string> ConfirmAccountByEmailAsync(string token, string userId)
        {
            var existUser = await _userManager.FindByIdAsync( userId );
            if (existUser == null) {
                return "Oops, Al parecer su cuenta no pudo ser verificada. Favor intente de nuevo mas tarde.";
            }
            var tokerVery = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(existUser, tokerVery);
            if (!result.Succeeded)
            {
                return "Oops, Al parecer a ocurrido un error al verificar su cuenta favor intente de nuevo mas tarde";
            }

            existUser.BlockedEmailSending = null;
            existUser.IsActive = true;
            await _userManager.UpdateSecurityStampAsync(existUser);
            return "Cuenta confirmada con exito, procesada autenticarse y disfrute de Real State App.";

        }

        public async Task<UserResponseDto> ForgoutPasswordAsync(
            ForgoutPasswordDto forgoutPasswordDto)
        {
            var response = new UserResponseDto
            {
                Errors = new List<string>(),
                HasError = false,
                Roles = null!,
            };
            var existUser = await _userManager.FindByNameAsync(forgoutPasswordDto.UserName);
            var validate = ValidateFields(response, forgoutPasswordDto, existUser!);
            if (validate != null && validate.HasError) return validate;
            
            if(existUser!.EmailConfirmed && existUser.IsActive)
            {
                var generateTokenResetPassword = await _generateTokens.GenerateTokenResetPassword(existUser,forgoutPasswordDto.Origin);
                if(string.IsNullOrWhiteSpace(generateTokenResetPassword))
                {
                    response.HasError = true;
                    response.Errors.Add("La solicitud no puede ser procesada de momento favor intente lo de nuevo mas tarde.");
                    return response;
                }
                var sendEmail = await _emailServices.SendEmailAsync(new MessageDto
                {
                    To = existUser.Email!,
                    Subject = "Real State App",
                    Body = "<div style = 'background:#4f46e5;color:white;padding:20px;" +
                    "text-align:center;font-size:20px;' >" +
                               "<h2> Real State App </h2> " +
                               "<p> Ha solicitado un cambio de contraseña para su cuenta </p>" +
                               $"<p style = 'color:#fff;' >{generateTokenResetPassword}<p>" +
                               "<p><b>Nota:</b> Si usted no ha realizado esta solicitud ignore este mensaje.</p>" +
                               " </div>"

                });
                if (!sendEmail)
                {
                    response.HasError = true;
                    response.Errors.Add("Oops," +
                        " Al parecer este servicio no se encuentra disponible de momento, " +
                        "favor intentelo de nuevo mas tarde.");
                    return response;
                }


            }

            return response;
        }

        public async Task<UserResponseDto> LoginUser(LoginDto loginDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Errors = new List<string>(),
                Roles = new List<string>()
            };

            var existUser = await _userManager.FindByNameAsync(loginDto.EmailOrNameUser);
            if (existUser != null)
            {
                var validate = await ValidateLogin(response, existUser, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUser);
                response.Roles = roles.ToList();
                return response;
            }
            var existUserByEmail = await _userManager.FindByEmailAsync(loginDto.EmailOrNameUser);
            if (existUserByEmail != null)
            {
                var validate = await ValidateLogin(response, existUserByEmail, loginDto);
                if (validate != null && validate.HasError) return validate;
                var roles = await _userManager.GetRolesAsync(existUserByEmail);
                response.Roles = roles.ToList();

                return response;
            }
            response.HasError = true;
            response.Errors.Add("Las credenciales ingresadas son invalidas favor intente de nuevo.");
            return response;


        }

        public async Task<UserResponseDto> ResendActivationEmailAsync(
            ResendActivationEmailDto resendActivationEmailDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors = new List<string>(),
            };

            var existUser = await _userManager.FindByNameAsync(resendActivationEmailDto.UserName);
            await _userManager.UpdateSecurityStampAsync(existUser!);
            var generateTokens = await _generateTokens.GenerateTokenConfirmEmail(existUser!, resendActivationEmailDto.Origin);

            if(string.IsNullOrWhiteSpace(generateTokens))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer esta funcion no se encuentra disponible de momento." +
                    " Favor intentelo de nuevo mas tarde.");
                return response;
            }

            existUser!.EmailConfirmed = false;
            existUser.IsActive = false;
            var update =  await _userManager.UpdateAsync(existUser);

            if (update.Succeeded)
            {
                existUser.BlockedEmailSending = DateTimeOffset.UtcNow.AddMinutes(5);
                var send = await _emailServices.SendEmailAsync(new MessageDto
                {
                    To = existUser.Email!,
                    Subject = "Resl State App",
                    Body = "<div style = 'background:#4f46e5;color:white;padding:20px;text-align:center;font-size:20px;' >" +
                        "<h2> Real State App </h2> " +
                        "<p> Ha solicitado un email de confirmación para su cuenta </p>" +
                        $"<p style = 'color:#fff;' >{generateTokens}<p>" +
                        "<p><b>Nota:</b> Si usted no ha realizado esta solicitud ignore este mensaje.</p>" +
                        " </div>",
                });
                if (!send)
                {
                    response.HasError = true;
                    response.Errors.Add("Oops, Al parecer esta funcion no se encuentra disponible de momento." +
                        "Favor intente de nuevo mas tarde.");
                    return response;
                }

                await _userManager.UpdateAsync(existUser);
                return response;
            }

            response.HasError = true;
            response.Errors.Add("Oops, Al parecer esta funcion no se encuentra disponible de momento." +
                        "Favor intente de nuevo mas tarde.");
            return response;
        }

        public async Task<UserResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var response = new UserResponseDto
            {
                HasError = false,
                Roles = null!,
                Errors = new List<string>(),
            };

            var exitsUser = await  _userManager.FindByIdAsync(resetPasswordDto.Id);
            if (exitsUser == null)
            {

                response.HasError = true;
                response.Errors.Add("Inexistencia de cuenta, Este usuario  no se encuentra asociada a ninguna cuenta," +
                    " favor registrese y disfrute de Link Up.");
                return response;
            }
            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmNewPassword)
            {
                response.HasError = true;
                response.Errors.Add("Las contraseñas ingresadas deben ser iguales.");
                return response;
            }

            var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(resetPasswordDto.Token));
            var verifyResetPassword = await _userManager.ResetPasswordAsync(exitsUser, token, resetPasswordDto.NewPassword);
            if (!verifyResetPassword.Succeeded)
            {
                response.HasError = true;
                response.Errors.AddRange(verifyResetPassword.Errors.Select(e => e.Description).ToList());
                return response;
            }

            exitsUser.BlockedEmailSending = null;
            exitsUser.AccessFailedCount = 0;

            await _userManager.UpdateSecurityStampAsync(exitsUser);
            await _userManager.ResetAccessFailedCountAsync(exitsUser);
            await _userManager.SetLockoutEndDateAsync(exitsUser, null);
            return response;
        }
        #endregion

        #region methods validations 

        private UserResponseDto ValidateFields(UserResponseDto response, 
            ForgoutPasswordDto forgoutPasswordDto, AppUsers existUser)
        {
            #region validaciones
            if (string.IsNullOrWhiteSpace(forgoutPasswordDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer esta funcionalidad no se encuentra disponible de momento.Intente de nuevo mas tarde.");
                return response;
            }
            if (existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer no ha ingresado un nombre de usuario valido.");
                return response;
            }

            if (existUser.BlockedEmailSending.HasValue &&
                existUser.BlockedEmailSending!.Value > DateTimeOffset.UtcNow
                )
            {
                response.HasError = true;
                response.Errors.Add("Ha solicitado un correo de confirmacion o " +
                    "cambio de password recientemente por lo que debe esperar 5 minutos antes de solicitar otro.");
                return response;
            }
            #endregion

            return response;
        }

        private async Task<UserResponseDto> ValidateLogin(UserResponseDto response, 
            AppUsers user, LoginDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.EmailOrNameUser))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Los datos de acceso son inválidos.");
                return response;
            }
            if (!user.EmailConfirmed && !user.IsActive)
            {
                response.HasError = true;
                response.Errors.Add("Su cuenta se encuentra inactiva." +
                    " Debe activarla mediante el enlace enviado a su correo electrónico.");
                return response;
            }
            var rolesUser = await _userManager.GetRolesAsync(user);
            if (rolesUser.Contains(Roles.Desarrollador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("El usuario no tiene un rol válido asignado. Póngase en contacto con un administrador.");
                return response;
            }

            var verifyUser = await _signInManager.PasswordSignInAsync(user, dto.Password, false, true);
            if (!verifyUser.Succeeded)
            {
                response.HasError = true;
                if (verifyUser.IsLockedOut)
                {
                    response.Errors.Add("Su cuenta se encuentra bloqueada por multiples intentos repetidos fallidos." +
                        " Favor intente nuevamente dentro de 15 minutos o restableza su contraseña.");
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

        private async Task<UserResponseDto> ValidateResentEmailConfirm(UserResponseDto response, AppUsers existUser,
            ResendActivationEmailDto resendActivationEmailDto)
        {
            #region validate 
            if (string.IsNullOrWhiteSpace(resendActivationEmailDto.UserName)
                || string.IsNullOrWhiteSpace(resendActivationEmailDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Datos invalidos, favor complete la solicitud correctamente.");
                return response;
            }
            if (string.IsNullOrWhiteSpace(resendActivationEmailDto.Origin))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer esta opcion no se encuentra disponible de momento." +
                    " Favor intentelo de nuevo mas tarde.");
                return response;
            }
            
            if (existUser == null)
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer no ha ingresado un nombre de usuario valido. Favor intentelo de nuevo mas tarde.");
                return response;
            }
            if (existUser.BlockedEmailSending.HasValue &&
              existUser.BlockedEmailSending!.Value > DateTimeOffset.UtcNow
              )
            {
                response.HasError = true;
                response.Errors.Add("Ha solicitado un correo de confirmacion o " +
                    "cambio de password recientemente por lo que debe esperar 5 minutos antes de solicitar otro.");
                return response;
            }
            var roles = await _userManager.GetRolesAsync(existUser);
            if (roles.Contains(Roles.Administrador.ToString())
                || roles.Contains(Roles.Desarrollador.ToString()))
            {
                response.HasError = true;
                response.Errors.Add("Oops, Al parecer esta funcionalidad se encuentra bloqueada para su usuario. " +
                    "Favor comuniquese con un administrador.");
                return response;
            }
            #endregion

            return response;
        }

        #endregion
    }
}
