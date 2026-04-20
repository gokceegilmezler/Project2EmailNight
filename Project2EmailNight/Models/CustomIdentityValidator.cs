using Microsoft.AspNetCore.Identity;

namespace Project2EmailNight.Models
{
    public class CustomIdentityValidator:IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = "Parola minimum 6 karakter olmalıdır!"
            };
        }

        public override IdentityError PasswordRequiresLower ()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiredLower",
                Description = "Lütfen en az 1 küçük karakter girin."
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiredUpper",
                Description = "Lütfen en az 1 büyük karakter giriniz"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiredDigit",
                Description = "Lütfen en az 1 tane rakam giriniz."
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "Lütfen en az 1 tane sembol giriniz."
            };
        }

    }
}
