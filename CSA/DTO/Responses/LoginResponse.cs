namespace CSA.DTO.Responses;

public class LoginResponse : IResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }

    public LoginResponse()
    {
        this.Token = "";
        this.UserId = Guid.Empty;
    }

    public LoginResponse(string token)
    {
        this.Token = token;
        this.UserId = Guid.Empty;
    }

    public LoginResponse(Guid userId)
    {
        this.Token = "";
        this.UserId = userId;
    }

    public LoginResponse(string token, Guid userId)
    {
        this.Token = token;
        this.UserId = userId;
    }
}