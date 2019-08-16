using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}