using System.ComponentModel.DataAnnotations;

namespace IdentityDemo.Dtos
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}
