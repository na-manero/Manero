using System.ComponentModel.DataAnnotations;

namespace Manero.Web.ViewModels;

public class UserViewModel
{
    public string UserId { get; set; } = null!;

    [Required(ErrorMessage = "A first name is required")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "A last name is required")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "An email address is required")]
    [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Email not valid")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
    public string? ImageUrl { get; set; }
}