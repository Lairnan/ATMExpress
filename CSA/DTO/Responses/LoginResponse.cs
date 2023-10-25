using CSA.Entities;

namespace CSA.DTO.Responses;

public class LoginResponse
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
}