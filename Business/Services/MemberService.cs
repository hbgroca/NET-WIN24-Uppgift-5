using Business.Dtos;
using Business.Factories;
using Business.Helpers;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Business.Services;

public class MemberService(IMemberRepository memberRepository, IAddressService addressService) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IAddressService _addressService = addressService;

    // Create
    public async Task<MemberModel> CreateMemberAsync(MemberRegistrationform form)
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

            // Create the address
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

    // Delete
}
