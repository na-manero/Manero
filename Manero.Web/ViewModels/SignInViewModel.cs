using System.ComponentModel.DataAnnotations;

namespace Manero.Web.ViewModels;

public class SignInViewModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Enter your email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
