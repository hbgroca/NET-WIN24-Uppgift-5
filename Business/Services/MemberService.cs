using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Shared.Models;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, IAddressService addressService) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IAddressService _addressService = addressService;

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
            // Begin a new transaction
            await _memberRepository.BeginTransactionAsync();

            // Remap with factory
            var memberEntity = MemberFactory.Create(form);
            memberEntity.Id = GenerateGuid.NewGuid();

            // Create the address if not already exists
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
                throw new Exception("Error while creating the address");

            memberEntity.AddressId = address.Id;

            // Set the date created and updated
            memberEntity.DateCreated = DateOnly.FromDateTime(DateTime.Now);
            memberEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Create the client in dbcontext
            await _memberRepository.CreateAsync(memberEntity);

            // Save the changes
            var result = await _memberRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the client");

            // Commit the transaction
            await _memberRepository.CommitTransactionAsync();

            // Return the client
            return MemberFactory.Create(memberEntity);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Something went wrong when creating new member. Error msg: {ex.Message}");
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
        return member.Select(MemberFactory.Create);
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
            var member = await _memberRepository.GetOneAsync(x => x.Id == form.Id);

            if (member == null)
            {
                Debug.WriteLine("Client not found.");
                await _memberRepository.RollbackTransactionAsync();
                return false;
            }
            var imageUrl = member.ImageUrl;

            // Remap with factory
            var memberEntity = MemberFactory.Update(form, member);

            // Create the address
            var address = await _addressService.CreateAddressAsync(form.Street, form.ZipCode, form.City, form.Country);
            if (address == null)
            {
                Debug.WriteLine("An error occurred while updating the client: ");
                await _memberRepository.RollbackTransactionAsync();
                return false;
            }

            memberEntity.AddressId = address.Id;

            // Set the date updated
            memberEntity.DateUpdated = DateOnly.FromDateTime(DateTime.Now);

            // Update the client in dbcontext
            _memberRepository.Update(memberEntity);

            // Save the changes
            var result = await _memberRepository.SaveAsync();
            if (result == 0)
                throw new Exception("Error while saving the client");

            // Commit the transaction
            await _memberRepository.CommitTransactionAsync();

            /// Remove image
            if (form.ProfilePicture is not null && imageUrl is not null)
            {
                var cutString = $"{Environment.CurrentDirectory}\\wwwroot\\uploaded\\clients\\{imageUrl.Substring(18)}";
                Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
                if (File.Exists(cutString))
                {
                    Debug.WriteLine($"!!! - Trying to remove file: {cutString}");
                    File.Delete(cutString);
                }
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine("An error occurred while updating the client: ", ex);
            return false;
        }
    }


    // Delete
}
