using System.Net.Http.Headers;
using System.Text;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using Newtonsoft.Json;

namespace CSA.DTO;

public static class RequestHandler
{
    /// <summary>
    /// Do request on server
    /// </summary>
    /// <param name="requestType">Type request</param>
    /// <param name="content">Non serializable data</param>
    /// <param name="path">path to controller</param>
    /// <param name="token">authorization token</param>
    /// <typeparam name="T">Any request</typeparam>
    /// <returns>ApiResponse</returns>
    /// <exception cref="ArgumentNullException">Request type equals null</exception>
    public static async Task<ApiResponse?> DoRequest<T>(RequestType requestType, T content, string path, string token = "")
        where T : IRequest
    {
        using var httpClient = new HttpClient();
        if(!string.IsNullOrWhiteSpace(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        httpClient.SetDefaultHeaders();

        var jsonCont = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        
        var httpResponseMessage = requestType switch
        {
            RequestType.Get => await httpClient.GetAsync($"{Settings.BaseUrl}/{path}"),
            RequestType.Post => await httpClient.PostAsync($"{Settings.BaseUrl}/{path}", jsonCont),
            RequestType.Put => await httpClient.PutAsync($"{Settings.BaseUrl}/{path}", jsonCont),
            RequestType.Patch => await httpClient.PatchAsync($"{Settings.BaseUrl}/{path}", jsonCont),
            RequestType.Delete => await httpClient.DeleteAsync($"{Settings.BaseUrl}/{path}"),
            _ => throw new ArgumentNullException(nameof(requestType), "RequestType not found")
        };
        
        var jsonStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ApiResponse>(jsonStr);
        return response;
    }

    internal static void SetDefaultHeaders(this HttpClient httpClient)
    {
        httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        httpClient.DefaultRequestHeaders.Add("Host", Settings.BaseUrl.Authority);
    }
}