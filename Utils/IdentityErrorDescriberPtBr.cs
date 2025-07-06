using Microsoft.AspNetCore.Identity;

namespace LojaOnline.Utils
{
    public class IdentityErrorDescriberPtBr : IdentityErrorDescriber
    {
        public override IdentityError PasswordTooShort(int length)
            => new IdentityError { Code = nameof(PasswordTooShort), Description = $"As senhas devem ter pelo menos {length} caracteres." };

        public override IdentityError PasswordRequiresNonAlphanumeric()
            => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "As senhas devem conter ao menos um caractere não alfanumérico." };

        public override IdentityError PasswordRequiresLower()
            => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "As senhas devem conter ao menos uma letra minúscula ('a'-'z')." };

        public override IdentityError PasswordRequiresUpper()
            => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "As senhas devem conter ao menos uma letra maiúscula ('A'-'Z')." };

        // Adicione outros métodos sobrescritos se quiser traduzir mais mensagens.
    }
}