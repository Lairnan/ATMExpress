using System.Collections.ObjectModel;
using AccessHub.Models;
using CSA.DTO.Responses;

namespace AccessHub.BehaviorsFiles;

public static class LogonHelper
{
    public static readonly ObservableCollection<UserToken> UsersToken = new();

    public static UserToken? GetUserAuthorize(UserToken userToken)
    {
        UserToken? user = new();
        if (!string.IsNullOrWhiteSpace(userToken.Token)) user = UsersToken.FirstOrDefault(s => s.Token == userToken.Token);
        if (user == null && userToken.UserId != Guid.Empty) user = UsersToken.FirstOrDefault(s => s.UserId.Equals(userToken.UserId));
        
        return user;
    }
}