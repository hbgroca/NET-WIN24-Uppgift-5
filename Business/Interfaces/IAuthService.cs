using Domain.Models;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<bool> AuthenticateAsync(MemberLoginFormModel form);
        Task<bool> LogoutAsync();
        Task<bool> SignUpAsync(MemberSignUpFormModel form);
    }
}