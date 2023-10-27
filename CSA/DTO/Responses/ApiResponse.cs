namespace CSA.DTO.Responses;

public class ApiResponse : IResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
}