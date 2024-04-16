using ASPNET_MVC.Models;
using ASPNET_MVC.ViewModels;
using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ASPNET_MVC.Controllers;

public class AccountController : Controller
{
    private readonly UserService _userService;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly DataContext _context;


    public AccountController(UserService userService, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager, DataContext context)
    {
        _userService = userService;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }










    [Authorize]
    [Route("/account")]
    [HttpGet]
    public async Task<IActionResult> Details()
    {
        if (!User.Identity!.IsAuthenticated)
        {
            Console.WriteLine("User is not signed in");
            var claims = User.Claims.ToList();
            foreach (var claim in claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            return RedirectToAction("SignIn", "Auth");

        }

        

        

        Console.WriteLine("Details action hit successfully");

        var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);

        var viewModel = new AccountDetailsViewModel()
        {
            BasicInfo = new AccountDetailsBasicInfoModel
            {
                FirstName = user!.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                Biography = user.Biography,
            },

            AddressInfo = new AccountDetailsAddressInfoModel
            {
                StreetName = user.Address?.StreetName!,
                City = user.Address?.City!,
                PostalCode = user.Address?.PostalCode!
            }
        };

        return View(viewModel);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateBasicInfo(AccountDetailsViewModel model)
    {

        ModelState.Remove("AddressInfo.StreetName");
        ModelState.Remove("AddressInfo.PostalCode");
        ModelState.Remove("AddressInfo.City");

        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.FirstName = model.BasicInfo!.FirstName;
                user.LastName = model.BasicInfo!.LastName;
                user.Email = model.BasicInfo!.Email;
                user.PhoneNumber = model.BasicInfo!.PhoneNumber;
                user.UserName = model.BasicInfo!.Email;
                user.Biography = model.BasicInfo!.Biography;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    ViewData["ErrorMessage"] = "Saved basic info";
                }
                else
                {
                    ViewData["ErrorMessage"] = "Unable to save basic info changes";
                }
            }
        }
        else
        {
            ViewData["ErrorMessage"] = "Unable to save basic info changes";
        }

        
        return RedirectToAction("Details", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> UpdateAddressInfo(AccountDetailsViewModel model)
    {

        ModelState.Remove("BasicInfo.FirstName");
        ModelState.Remove("BasicInfo.LastName");
        ModelState.Remove("BasicInfo.Email");

        if (ModelState.IsValid)
        {
            var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = await _context.Users.Include(i => i.Address).FirstOrDefaultAsync(x => x.Id == nameIdentifier);
            if (user != null)
            {
                if (user.Address != null)
                {
                    user.Address.StreetName = model.AddressInfo!.StreetName;
                    user.Address.PostalCode = model.AddressInfo!.PostalCode;
                    user.Address.City = model.AddressInfo!.City;
                }
                else
                {
                    user.Address = new AddressEntity
                    {
                        StreetName = model.AddressInfo!.StreetName,
                        PostalCode = model.AddressInfo!.PostalCode,
                        City = model.AddressInfo!.City,
                    };
                }

                _context.Update(user);
                await _context.SaveChangesAsync();
            

               
            }
        }
        else
        {
            ViewData["ErrorMessage"] = "Unable to save address info changes";
        }


        return RedirectToAction("Details", "Account");
    }
}
