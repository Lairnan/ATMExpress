namespace CSA.DTO.Requests;

public class LoginRequest : IRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    public LoginRequest(string login, string password)
    {
        this.Login = login;
        this.Password = password;
    }
}