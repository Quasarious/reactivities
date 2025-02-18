using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,12}$", ErrorMessage = "Пароль должен быть от 4 до 12 символов и содержать буквы и цифры")]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}