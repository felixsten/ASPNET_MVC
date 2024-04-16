using System.ComponentModel.DataAnnotations;

namespace ASPNET_MVC.Models;

public class AccountDetailsAddressInfoModel
{
    [Display(Name = "Address 1", Order = 0)]
    [Required(ErrorMessage = "Invalid address")]
    public string StreetName { get; set; } = null!;
    
    [Display(Name = "Postal code", Order = 1)]
    [Required(ErrorMessage = "Invalid first name")]
    [DataType(DataType.PostalCode)]
    public string PostalCode { get; set; } = null!;

    [Display(Name = "City", Order = 2)]
    [Required(ErrorMessage = "Invalid last name")]
    public string City { get; set; } = null!;

}
