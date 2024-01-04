using CSA.DTO.Responses;

namespace CSA.DTO.Requests;

public class ApiRequest : IRequest
{
    public string Token { get; set; }
    public Guid UserId { get; set; }

    public ApiRequest(string token, Guid userId)
    {
        this.Token = token;
        this.UserId = userId;
    }

    public ApiRequest(LoginResponse user)
    {
        this.Token = user.Token;
        this.UserId = user.UserId;
    }
}