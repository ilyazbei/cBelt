using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class RegisterViewModel : BaseEntity
    {
        [Required(ErrorMessage="First Name is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name can only contain letters")]
        [MinLength(3, ErrorMessage="First Name must be 3 characters long")]
        public string firstName { get; set; }
        
        [Required(ErrorMessage="Last Name is Required")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name can only contain letters")]
        [MinLength(3, ErrorMessage="Last Name must be 3 characters long")]
        public string lastName { get; set; }
        
        [Required(ErrorMessage="Email is Required")]
        [RegularExpression(@"[\w+\-.]+@[a-z\d\-]+(\.[a-z\d\-]+)*\.[a-z]+", ErrorMessage = "Incorrect Email format")]
        public string email { get; set; }
        
        [Required(ErrorMessage="Password is Required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Password must be 8 characters long")]
        public string password { get; set; }

        // [Required(ErrorMessage="Confirm Password is Required")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage="Password and confirmation must match.")]
        public string confirm_password { get; set; }
    }
}