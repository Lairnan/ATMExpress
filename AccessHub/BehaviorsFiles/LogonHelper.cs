using CSA.DTO.Responses;

namespace AccessHub.BehaviorsFiles;

public static class LogonHelper
{
    public static readonly Dictionary<LoginResponse, bool> InvalidTokens = new(new LoginResponseEqualityComparer());
}

public class LoginResponseEqualityComparer : IEqualityComparer<LoginResponse>
{
    public bool Equals(LoginResponse? x, LoginResponse? y)
    {
        if (x == null || y == null) return false;
        
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Token == y.Token || x.UserId.Equals(y.UserId);
    }

    public int GetHashCode(LoginResponse obj)
    {
        return obj.UserId.ToString().GetHashCode(StringComparison.OrdinalIgnoreCase);
    }
}