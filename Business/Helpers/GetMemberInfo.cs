using Business.Interfaces;

namespace Business.Helpers;

public class GetMemberInfo(IMemberService memberService)
{
    private readonly IMemberService _memberService = memberService;

    public async Task<DateTime> GetCreateDate(string userId)
    {
        var user = await _memberService.GetMemberAsync(user => user.Id == userId);
        DateTime createDate = DateTime.Parse(user.DateCreated.ToString());

        return createDate;
    }
}
