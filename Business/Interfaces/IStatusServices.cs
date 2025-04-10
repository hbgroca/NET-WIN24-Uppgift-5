using Business.Dtos;
using Business.Models;

namespace Business.Interfaces;
public interface IStatusServices
{
    Task<bool> AddStatus(ClientStatusFormModel status);
    Task<bool> AddStatus(MemberStatusFormModel status);
    Task<IEnumerable<ClientStatusModel>> GetClientStatuses();
    Task<IEnumerable<MemberStatusModel>> GetMemberStatuses();
    Task<bool> RemoveClientStatus(int id);
    Task<bool> RemoveMemberStatus(int id);
}