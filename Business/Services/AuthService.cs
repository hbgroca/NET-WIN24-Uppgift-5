using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;



    public async Task<bool> AuthenticateAsync(MemberLoginFormModel form)
    {
        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);

        return result.Succeeded;
    }

    public async Task<bool> SignUpAsync(MemberSignUpFormModel form){

        // Temporary Factory
        var entity = new MemberEntity();
        entity.UserName = form.Email;
        entity.Title = "Junior";
        entity.FirstName = form.FirstName;
        entity.LastName = form.LastName;
        entity.Email = form.Email;
        entity.AddressId = 1;
        entity.Status = "Active";
     
        entity.PhoneNumber = "070123456";
        entity.BirthDate = DateOnly.Parse("01-01-1900");

        var result = await _userManager.CreateAsync(entity, form.Password);

        return result.Succeeded;
    }


    public async Task<bool> LogoutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
        }
        catch
        {
            Debug.WriteLine("Something went wrong while signing out.");
            return false;
        }
        return true;
    }
}
