using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Infraestructure.Identity.Errors
{
    public sealed class SpanishIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError PasswordRequiresDigit()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresDigit),
                Description = "La contraseña debe contener al menos un número (0-9)."
            };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresLower),
                Description = "La contraseña debe contener al menos una letra minúscula (a-z)."
            };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUpper),
                Description = "La contraseña debe contener al menos una letra mayúscula (A-Z)."
            };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError
            {
                Code = nameof(PasswordRequiresNonAlphanumeric),
                Description = "La contraseña debe contener al menos un carácter especial (por ejemplo: @, #, $, !)."
            };

        public override IdentityError PasswordTooShort(int length)
            => new IdentityError
            {
                Code = nameof(PasswordTooShort),
                Description = $"La contraseña debe tener al menos {length} caracteres."
            };

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
            => new IdentityError
            {
                Code = nameof(PasswordRequiresUniqueChars),
                Description = $"La contraseña debe contener al menos {uniqueChars} caracteres distintos."
            };

        public override IdentityError InvalidUserName(string? userName)
            => new IdentityError
            {
                Code = nameof(InvalidUserName),
                Description = "El nombre de usuario solo puede contener letras sin acentos, números y los caracteres - . _ @ +, sin espacios."
            };

        public override IdentityError InvalidEmail(string? email)
            => new IdentityError
            {
                Code = nameof(InvalidEmail),
                Description = "Debe ingresar un correo electrónico válido."
            };

        public override IdentityError DuplicateUserName(string userName)
            => new IdentityError
            {
                Code = nameof(DuplicateUserName),
                Description = "Ya existe un usuario registrado con este nombre de usuario."
            };

        public override IdentityError DuplicateEmail(string email)
            => new IdentityError
            {
                Code = nameof(DuplicateEmail),
                Description = "Ya existe un usuario registrado con este correo electrónico."
            };

        public override IdentityError PasswordMismatch()
            => new IdentityError
            {
                Code = nameof(PasswordMismatch),
                Description = "La contraseña actual es incorrecta."
            };

        public override IdentityError InvalidToken()
            => new IdentityError
            {
                Code = nameof(InvalidToken),
                Description = "El enlace utilizado no es válido o ha expirado. Solicite uno nuevo."
            };

        public override IdentityError DefaultError()
            => new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Ha ocurrido un error inesperado al procesar la solicitud. Intente nuevamente más tarde."
            };
    }
}
