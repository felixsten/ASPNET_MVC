using ASPNET_MVC.Models;
using Infrastructure.Entities;

namespace ASPNET_MVC.ViewModels;

public class AccountDetailsViewModel
{

    public string Title { get; set; } = "Account Details";
    public AccountDetailsBasicInfoModel BasicInfo { get; set; } = new AccountDetailsBasicInfoModel();
    public AccountDetailsAddressInfoModel AddressInfo { get; set; } = new AccountDetailsAddressInfoModel();
    
}
