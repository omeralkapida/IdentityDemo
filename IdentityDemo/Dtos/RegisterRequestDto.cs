using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Dtos
{
    public class RegisterRequestDto
    {

        [Required(ErrorMessage = "Ad soyad alanı zorunludur.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı alanı zorunludur.")]
        [RegularExpression("^[a-z0-9_\\.]{4,20}$", ErrorMessage = "Kullanıcı adı küçük harf, rakam ve nokta/alt çizgi içerebilir. Türkçe karakter içeremez.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        public string Password { get; set; }
    }
}
