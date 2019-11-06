using System.ComponentModel.DataAnnotations;

namespace App.Caliset.Users.Dto
{
    public class ResetPasswordDto
    {
   

        [Required]
        public long UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
