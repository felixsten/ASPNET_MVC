using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC.Models;

public class AccountDetailsBasicInfoModel
{

    [DataType(DataType.ImageUrl)]
    public string? ProfileImage { get; set; }


    [Display(Name = "First name", Order = 0)]
    [Required(ErrorMessage = "Invalid first name")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last name", Order = 1)]
    [Required(ErrorMessage = "Invalid last name")]
    public string LastName { get; set; } = null!;

    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email", Order = 2)]
    [Required(ErrorMessage = "Invalid email address")]
    public string Email { get; set; } = null!;

    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Phone", Order = 3)]
    public string? PhoneNumber { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Bio", Order = 4)]
    public string? Biography { get; set; }
}
