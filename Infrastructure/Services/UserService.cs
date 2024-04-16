

using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Helpers;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Infrastructure.Services;

public class UserService(UserRepository repository, AddressService addressService, UserManager<UserEntity> userManager, SignInManager<UserEntity> signInManager)
{
    private readonly UserRepository _repository = repository;
    private readonly AddressService _addressService = addressService;
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;





    public async Task<ResponseResult> CreateUserAsync(SignUpModel model)
    {
        try
        {
            var user = new UserEntity
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User created successfully
                return ResponseFactory.Ok("User was created successfully");
            }
            else
            {
                // User creation failed, return errors
                return ResponseFactory.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }


    public async Task<ResponseResult> SignInUserAsync(SignInModel model)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (signInResult.Succeeded)
                {
                    // Password is correct, return user entity
                    return ResponseFactory.Ok(user);
                }
            }

            return ResponseFactory.Error("Incorrect email or password");
        }
        catch (Exception ex)
        {
            return ResponseFactory.Error(ex.Message);
        }
    }
}
