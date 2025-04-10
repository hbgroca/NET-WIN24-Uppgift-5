using Business.Dtos;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AuthenticateAdminAsync(MemberLoginFormModel form);
        Task<bool> AuthenticateAsync(MemberLoginFormModel form);
        Task<bool> LogoutAsync();
        Task<bool> SignUpAsync(MemberSignUpFormModel form);
    }
}