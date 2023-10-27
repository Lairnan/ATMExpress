namespace CSA.DTO.Responses;

public class LoginResponse : IResponse
{
    public required string Token { get; init; }
    public Guid UserId { get; init; }
}