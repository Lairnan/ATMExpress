namespace CSA.DTO.Requests;

public class RegisterRequest : IRequest
{
    public string Login { get; set; }
    public string Password { get; set; }

    public RegisterRequest(string login, string password)
    {
        this.Login = login;
        this.Password = password;
    }
}