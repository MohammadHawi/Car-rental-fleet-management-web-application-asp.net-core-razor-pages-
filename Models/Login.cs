using System.ComponentModel.DataAnnotations;

namespace V4.Models
{
    public class Login
    {
        
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }





    }
}
