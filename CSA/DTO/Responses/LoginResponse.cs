namespace CSA.DTO.Responses;

public class LoginResponse
{
    public required string Token { get; init; }
    public Guid UserId { get; init; }
}