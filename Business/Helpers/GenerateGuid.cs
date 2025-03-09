using System.Diagnostics;

namespace Business.Helpers;

public static class GenerateGuid
{
    public static Guid NewGuid()
    {
        var guid = Guid.NewGuid();
        Debug.WriteLine($"GenerateGuid generated: {guid}");
        return guid;
    }
}
