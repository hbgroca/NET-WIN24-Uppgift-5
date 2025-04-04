using Business.Factories;
using Business.Interfaces;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Business.Services;

public class AuthService(INotificationSerivces notificationSerivces, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IAddressService addressService) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IAddressService _addressService = addressService;
    private readonly INotificationSerivces _notificationSerivces = notificationSerivces;

    public async Task<bool> AuthenticateAsync(MemberLoginFormModel form)
    {
        var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(form.Email!);
            if (user != null)
            {
                string Message = $"{user.FirstName} {user.LastName} is live!";
                await _notificationSerivces.AddNotificationAsync(1, Message, user.Id, user.ImageUrl!,1);
            }
        }
        return result.Succeeded;
    }

    public async Task<bool> SignUpAsync(MemberSignUpFormModel form){

        // Do the facotry thing :)
        var entity = MemberFactory.Create(form);

        // Create the address if not already exists
        var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
        if (address == null)
            throw new Exception("Error while creating the address");

        entity.AddressId = address.Id;

        // Set the date created and updated
        entity.DateCreated = DateOnly.FromDateTime(DateTime.Now);
        entity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

        var result = await _userManager.CreateAsync(entity, form.Password);

        if (result.Succeeded)
        {
            string Message = $"Welcome our newest member {entity.FirstName} {entity.LastName}!";
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
