using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, IAddressService addressService, UserManager<MemberEntity> userManager, IImageServices imageServices) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IAddressService _addressService = addressService;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IImageServices _imageServices = imageServices;

    // Create
    public async Task<MemberModel> CreateMemberAsync(AddMemberFormModel form)
    {
        if (form == null)
        {
            Debug.WriteLine("Registrationform missing.");
            return null!;
        }
        try
        {
            // Store image
            if (form.ProfilePicture != null && form.ProfilePicture.Length >= 0)
            {
                var imagePath = await _imageServices.Create(form.ProfilePicture, "members");
                if (!string.IsNullOrEmpty(imagePath))
                    form.ImageName = imagePath;
            }
            else
                form.ImageName = $"/images/defaultmember.png";

            // Remap with factory
            var memberEntity = MemberFactory.Create(form);
            memberEntity.Id = GenerateGuid.NewGuid().ToString();

            // Create the address if not already exists
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
                throw new Exception("Error while creating the address");

            memberEntity.AddressId = address.Id;

            // Set the date created and updated
            memberEntity.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            memberEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Create the member
            memberEntity.UserName = memberEntity.Email;
            var result = await _userManager.CreateAsync(memberEntity, "BytMig123!");
            if(result.Succeeded)
                return MemberFactory.Create(memberEntity);

            // If we get here something went wrong... Sad face :(
            _imageServices.Delete(form.ImageName!);
            return null!;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Something went wrong when creating new member. Error msg: {ex.Message}");
            _imageServices.Delete(form.ImageName!);
            return null!;
        }
    }


    // Read
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
