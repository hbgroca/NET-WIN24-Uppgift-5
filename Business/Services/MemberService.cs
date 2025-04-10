using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberService(INotificationSerivces notificationSerivces ,IMemberRepository memberRepository, IAddressService addressService, UserManager<MemberEntity> userManager, IImageServices imageServices) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IAddressService _addressService = addressService;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IImageServices _imageServices = imageServices;
    private readonly INotificationSerivces _notificationsServices = notificationSerivces;

    // Create
    // Is created throu AuthService


    // Read
    public async Task<bool> IsMemberAdmin(string userName)
    {
        var user = await _memberRepository.GetOneAsync(u => u.UserName == userName);
        if (user == null)
            return false;

        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return true;

        return false;
    }

    public async Task<MemberModel> GetMemberAsync(Expression<Func<MemberEntity, bool>> expression)
    {
        var member = await _memberRepository.GetOneAsync(expression);
        if (member == null)
            return null!;
        return MemberFactory.Create(member);
    }

    public async Task<IEnumerable<MemberModel>> GetMembersAsync(Expression<Func<MemberEntity, bool>> expression)
    {
        var member = await _memberRepository.GetAllAsync(expression);
        if (member == null)
            return null!;
        return member.Select(MemberFactory.Create);
    }

    public async Task<IEnumerable<MemberModel>> GetAllMembersAsync()
    {
        var member = await _memberRepository.GetAllAsync();
        if (member == null)
            return null!;

        Debug.WriteLine($"! - Returning {member.Count()}");
        return member.Select(MemberFactory.CreateWithProjects);
    }


    // Update
    public async Task<bool> UpdateMember(EditMemberFormModel form)
    {
        if (form == null)
        {
            Debug.WriteLine("EditMemberFormModel missing.");
            return false;
        }

        try
        {
            // Begin a new transaction
            await _memberRepository.BeginTransactionAsync();

            // Get the client
            var member = await _memberRepository.GetOneAsync(x => x.Id == form.Id.ToString());

            if (member == null)
            {
                Debug.WriteLine("Client not found.");
                await _memberRepository.RollbackTransactionAsync();
                return false;
            }

            // Update image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var imagePath = await _imageServices.Update(form.ProfilePicture, "members", member.ImageUrl!);
                if (!string.IsNullOrEmpty(imagePath))
                    form.ImageName = imagePath;
            }

            // Remap with factory
            var memberEntity = MemberFactory.Update(form, member);

            // Create the address
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
            {
                Debug.WriteLine("An error occurred while updating the member: ");
                await _memberRepository.RollbackTransactionAsync();
                return false;
            }

            memberEntity.AddressId = address.Id;

            // Set the date updated
            memberEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Update the member in dbcontext
            _memberRepository.Update(memberEntity);

            // Save the changes
            var result = await _memberRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the member");

            // Commit the transaction
            await _memberRepository.CommitTransactionAsync();

            // Send notifcations to other team members
            if (result > 0)
            {
                string Message = $"Member {memberEntity.FirstName} {memberEntity.LastName} was updated.";
                await _notificationsServices.AddNotificationAsync(3, Message, memberEntity.Id, memberEntity.ImageUrl!, 2);
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating the member: ", ex);
            _imageServices.Delete(form.ImageName!);
            return false;
        }
    }



    // Delete
    public async Task<bool> Delete(Guid id)
    {
        try
        {
            // Get the entity
            var entity = await _memberRepository.GetOneAsync(x => x.Id == id.ToString());
            if (entity == null)
                return false;

            // Delete
            await _userManager.DeleteAsync(entity);

            // Remove image
            _imageServices.Delete(entity.ImageUrl!);

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
