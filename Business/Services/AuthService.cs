using Business.Dtos;
using Business.Factories;
using Business.Hubs;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Business.Services;

public class AuthService(INotificationSerivces notificationSerivces, SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IAddressService addressService, IMemberService memberService, IHubContext<NotificationHub> hubContext) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IAddressService _addressService = addressService;
    private readonly IMemberService _memberService = memberService;
    private readonly INotificationSerivces _notificationSerivces = notificationSerivces;
    private readonly IHubContext<NotificationHub> _notificationHub = hubContext;


    // Sign in
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



    // Sign up
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
            var user = _userManager.FindByEmailAsync(form.Email);

            // Send a notification to admins 
            string Message = $"Member {entity.FirstName} {entity.LastName} has signed up!";

            var notification = new NotificationEntity
            {
                Message = Message,
                Created = DateTime.Now,
                Image = entity.ImageUrl ?? "",
                TargetGroupId = 2, // Admins only
                NotificationTypeId = 3, // Team Member
            };

            await _notificationSerivces.AddNotificationAsync(notification, user.Id.ToString());
        }

        return result.Succeeded;
    }


    // Sign out
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
