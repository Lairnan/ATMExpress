using System.Net.Http.Headers;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using Newtonsoft.Json;

namespace CSA.DTO;

public static class RequestHandler
{
    public static async Task<IResponse?> DoRequest<T>(RequestType requestType, T content, string path, string token = "")
        where T : IRequest
    {
        using var httpClient = new HttpClient();
        if(!string.IsNullOrWhiteSpace(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        
        httpClient.DefaultRequestHeaders.Add("", "");
        
        var httpResponseMessage = requestType switch
        {
            RequestType.Get => await httpClient.GetAsync($"{Settings.BaseUrl}/{path}"),
            RequestType.Post => await httpClient.PostAsync($"{Settings.BaseUrl}/{path}", null),
            RequestType.Put => await httpClient.PutAsync($"{Settings.BaseUrl}/{path}", null),
            RequestType.Patch => await httpClient.PatchAsync($"{Settings.BaseUrl}/{path}", null),
            RequestType.Delete => await httpClient.DeleteAsync($"{Settings.BaseUrl}/{path}"),
            _ => throw new ArgumentNullException(nameof(requestType), "RequestType not found")
        };


        if (!httpResponseMessage.IsSuccessStatusCode)
            return new ApiResponse
            {
                Success = false,
                Message = $"Request Failed: {httpResponseMessage.ReasonPhrase}",
                Data = ""
            };
        
        var jsonStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<IResponse>(jsonStr);
        return response;
    }
}