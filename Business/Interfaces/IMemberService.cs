using Business.Dtos;
using Business.Models;
using Data.Entities;
using Shared.Models;
using System.Linq.Expressions;

namespace Business.Interfaces;
public interface IMemberService
{
    Task<MemberModel> CreateMemberAsync(AddMemberFormModel form);
    Task<IEnumerable<MemberModel>> GetAllMembersAsync();
    Task<IEnumerable<MemberModel>> GetMembersAsync(Expression<Func<MemberEntity, bool>> expression);
    Task<MemberModel> GetMemberAsync(Expression<Func<MemberEntity, bool>> expression);
    Task<bool> UpdateMember(EditMemberFormModel form);
}