using System.ComponentModel.DataAnnotations;

namespace Manero.Web.ViewModels;

public class SignUpViewModel
{
    [Required]
    [Display(Name = "First Name", Prompt = "Name")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name", Prompt = "Surname")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email", Prompt = "Email")]
    [RegularExpression(@"^[a-zA-Z0-9.!#$%&'+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?).[a-zA-Z]{2,}$", ErrorMessage = "Provide a valid email address.")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;


    [Required]
    [Display(Name = "Password", Prompt = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Confirm Password", Prompt = "Confirm password")]
    [Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}