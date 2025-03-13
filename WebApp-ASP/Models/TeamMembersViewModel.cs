using Business.Interfaces;
using Business.Models;
using Shared.Models;

namespace WebApp_ASP.Models;

public class TeamMembersViewModel()
{
    public IEnumerable<MemberModel> Members { get; set; } = new List<MemberModel>();
    public MemberRegistrationFormModel MemberRegistrationForm { get; set; } = new();
    public bool IsUpdate = false;
    public bool RegistrationFormInvalid = false;
}
