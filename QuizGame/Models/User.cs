using System.ComponentModel.DataAnnotations;

namespace QuizGame.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        public required string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
