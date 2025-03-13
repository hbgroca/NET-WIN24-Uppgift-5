using Business.Dtos;
using Business.Models;
using Data.Entities;
using Shared.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;
public interface IMemberService
{
    Task<MemberModel> CreateMemberAsync(MemberRegistrationFormModel form);
    Task<IEnumerable<MemberModel>> GetAllMembersAsync();
    Task<IEnumerable<MemberModel>> GetMembersAsync(Expression<Func<MemberEntity, bool>> expression);
    Task<MemberModel> GetMemberAsync(Expression<Func<MemberEntity, bool>> expression);
    MemberRegistrationFormModel CreateRegistrationUpdateForm(MemberModel member);
}