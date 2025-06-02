using System.ComponentModel.DataAnnotations;

namespace IdentityDemoSysteam.Dtos
{
    public class ResetPasswordViewModel
    {
        public string UserId { get; set; } // Kullanıcının ID'si
        public string Token { get; set; }  // Şifre sıfırlama token'ı

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } // Yeni şifre alanı

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor.")]
        public string ConfirmPassword { get; set; } // Şifre tekrar alanı
    }

}
