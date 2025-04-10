using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class AuthService(INotificationSerivces notificationSerivces, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IAddressService addressService, IMemberService memberService) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IAddressService _addressService = addressService;
    private readonly INotificationSerivces _notificationSerivces = notificationSerivces;
    private readonly IMemberService _memberService = memberService;

    public async Task<bool> AuthenticateAsync(MemberLoginFormModel form)
    {
        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
        return result.Succeeded;
    }

    public async Task<bool> AuthenticateAdminAsync(MemberLoginFormModel form)
    {
        var isAdmin = await _memberService.IsMemberAdmin(form.Email);
        if (!isAdmin)
            return false;

        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
        return result.Succeeded;
    }

    public async Task<bool> SignUpAsync(MemberSignUpFormModel form){

        // Do the facotry thing :)
        var entity = MemberFactory.Create(form);

        // Create the address if not already exists
        var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
        if (address == null)
            throw new Exception("Error while creating the address");

        // Set Address ID
        entity.AddressId = address.Id;

        // Create User
        var result = await _userManager.CreateAsync(entity, form.Password);
        if (result.Succeeded)
        {
            // Add the user to the default role
            await _userManager.AddToRoleAsync(entity, "Member");
            // Send a notification to admins 
            string Message = $"Member {entity.FirstName} {entity.LastName} has signed up!";
            await _notificationSerivces.AddNotificationAsync(3, Message, entity.Id, entity.ImageUrl!, 2);
        }

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
