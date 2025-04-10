using Business.Dtos;
using Business.Models;

namespace WebApp_ASP.Models;

public class ProjectViewModel
{
    public AddProjectFormModel AddProjectFormModel = new();
    public List<MemberModel> MembersInDb = [];
}
