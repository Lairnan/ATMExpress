using System.Net.Http.Headers;
using System.Text;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using Newtonsoft.Json;

namespace CSA.DTO.Handlers;

public static partial class RequestHandler
{
    /// <summary>
    /// Do request on server
    /// </summary>
    /// <param name="requestType">Type request</param>
    /// <param name="content">Non serializable data</param>
    /// <param name="path">Path to controller</param>
    /// <param name="headers">Headers to request server</param>
    /// <param name="token">authorization token</param>
    /// <typeparam name="T">Any request</typeparam>
    /// <returns>ApiResponse</returns>
    /// <exception cref="ArgumentNullException">Request type equals null</exception>
    public static async Task<ApiResponse?> DoRequest<T>(RequestType requestType, T content, string path, string token = "", HttpRequestHeaders? headers = null)
        where T : IRequest
    {
        if (string.IsNullOrWhiteSpace(path)) return new ApiResponse
            {
                Success = false,
                Message = "bad_request",
                Data = ""
            };

        if (!Uri.TryCreate(path, UriKind.Absolute, out _)) path = $"{Settings.BaseUrl}/{path}";
        
        using var httpClient = new HttpClient();
        
        if (headers != null) httpClient.SetDefaultHeadersByHeaders(headers);
        if (!string.IsNullOrWhiteSpace(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "");
        httpClient.SetDefaultHeaders();

        var jsonCont = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        
        var httpResponseMessage = requestType switch
        {
            RequestType.Get => await httpClient.GetAsync($"{path}"),
            RequestType.Post => await httpClient.PostAsync($"{path}", jsonCont),
            RequestType.Put => await httpClient.PutAsync($"{path}", jsonCont),
            RequestType.Patch => await httpClient.PatchAsync($"{path}", jsonCont),
            RequestType.Delete => await httpClient.DeleteAsync($"{path}"),
            _ => throw new ArgumentNullException(nameof(requestType), "RequestType not found")
        };
        
        var jsonStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ApiResponse>(jsonStr);
        return response;
    }

    internal static void SetDefaultHeaders(this HttpClient httpClient)
    {
        if (!httpClient.DefaultRequestHeaders.TryGetValues("Accept", out _))
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
        
        if (!httpClient.DefaultRequestHeaders.TryGetValues("Host", out _))
            httpClient.DefaultRequestHeaders.Add("Host", Settings.BaseUrl.Host);
    }

    private static void SetDefaultHeadersByHeaders(this HttpClient httpClient, HttpRequestHeaders headers)
    {
        httpClient.DefaultRequestHeaders.Clear();
        
        foreach (var header in headers)
        {
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }
    }
}