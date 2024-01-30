using AccessHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccessHub.BehaviorsFiles;

public class UserTokenAuthorizationFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var token = context.HttpContext.Request.Headers.Authorization.ToString();
        var userIdStr = context.HttpContext.Request.Headers["UserId"].ToString();
        if (string.IsNullOrWhiteSpace(token) && (string.IsNullOrWhiteSpace(userIdStr) || userIdStr.Equals(Guid.Empty.ToString()))) return;
        if(!string.IsNullOrWhiteSpace(token) && token.StartsWith("Bearer", StringComparison.OrdinalIgnoreCase))
            token = token.Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
        
        Guid? userId = null;
        if (!string.IsNullOrWhiteSpace(userIdStr)) userId = Guid.Parse(userIdStr);
        
        var userToken = LogonHelper.GetUserAuthorize(userId == null ? new UserToken(token) : new UserToken(token, userId));

        if (userToken is not { Valid: true }) context.Result = new UnauthorizedResult();
    }
}