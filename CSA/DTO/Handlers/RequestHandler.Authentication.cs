using CSA.DTO.Requests;
using CSA.DTO.Responses;
using Newtonsoft.Json;

namespace CSA.DTO.Handlers;

public partial class RequestHandler
{
    public static async Task<object> Login(LoginRequest request)
    {
        var response = await DoRequest(RequestType.Post, request, $"{Settings.BaseUrl}/auth/login");
        if(response is not { Success: true })
        {
            return response;
        }

        var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(response.Data)!;
        return loginResponse;
    }
    
    public static async Task<ApiResponse> Register(RegisterRequest request)
    {
        var response = await DoRequest(RequestType.Post, request, $"{Settings.BaseUrl}/auth/register");
        return response;
    }
    
    public static async Task<ApiResponse> Logout(ApiRequest request)
    {
        var response = await DoRequest(RequestType.Post, request, $"{Settings.BaseUrl}/auth/logout", request.Token);
        return response;
    }
}