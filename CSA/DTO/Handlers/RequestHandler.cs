using System.Net.Http.Headers;
using System.Text;
using CSA.DTO.Requests;
using CSA.DTO.Responses;
using Newtonsoft.Json;

namespace CSA.DTO.Handlers;

public static partial class RequestHandler
{
    /// <summary>
    /// Executes a request with the specified parameters and returns the response.
    /// </summary>
    /// <param name="requestType">The type of the request (GET, POST, PUT, DELETE).</param>
    /// <param name="content">The content of the request.</param>
    /// <param name="path">The path of the request.</param>
    /// <param name="token">The optional authentication token.</param>
    /// <param name="headers">The optional request headers.</param>
    /// <typeparam name="T">The type of the request content.</typeparam>
    /// <returns>The API response object.</returns>
    public static async Task<ApiResponse> DoRequest<T>(RequestType requestType, T content, string path, string token = "", HttpRequestHeaders? headers = null)
        where T : IRequest
    {
        if (string.IsNullOrWhiteSpace(path)) return new ApiResponse
            {
                Success = false,
                Message = "bad_request",
                Data = ""
            };

        if (!Uri.TryCreate(path, UriKind.Absolute, out _)) path = $"{Settings.BaseUrl}/{path}";
        
        var jsonCont = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        return await DoRequest(requestType, path, jsonCont, token, headers);
    }
    
    /// <summary>
    /// Executes a request of the specified type to the given path, with the provided login response and optional headers.
    /// </summary>
    /// <param name="requestType">The type of the HTTP request to be executed.</param>
    /// <param name="path">The path to which the request should be sent.</param>
    /// <param name="loginResponse">The login response object containing the token.</param>
    /// <param name="headers">Optional headers to be included in the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the API response, or null if the request failed.</returns>
    public static async Task<ApiResponse> DoRequest(RequestType requestType, string path, LoginResponse loginResponse, HttpRequestHeaders? headers = null)
    {
        if (string.IsNullOrWhiteSpace(path)) return new ApiResponse
        {
            Success = false,
            Message = "bad_request",
            Data = ""
        };

        if (!Uri.TryCreate(path, UriKind.Absolute, out _)) path = $"{Settings.BaseUrl}/{path}"; 
        return await DoRequest(requestType, path, null, loginResponse.Token, headers);
    }

    /// <summary>
    /// Executes an HTTP request with the specified request type, path, content, token, and optional headers.
    /// </summary>
    /// <param name="requestType">The type of the HTTP request (Get, Post, Put, Patch, or Delete).</param>
    /// <param name="path">The path of the API endpoint.</param>
    /// <param name="content">The HTTP content to send with the request (null for GET and DELETE requests).</param>
    /// <param name="token">The bearer token for the request authorization.</param>
    /// <param name="headers">Optional HTTP headers to include in the request.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
    /// <exception cref="ArgumentNullException">Bad request.</exception>
    private static async Task<ApiResponse> DoRequest(RequestType requestType, string path, HttpContent? content, string token,
        HttpRequestHeaders? headers = null)
    {
        using var httpClient = new HttpClient();
        
        if (headers != null) httpClient.SetDefaultHeadersByHeaders(headers);
        if (!string.IsNullOrWhiteSpace(token))
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        httpClient.SetDefaultHeaders();
        
        var httpResponseMessage = requestType switch
        {
            RequestType.Get => await httpClient.GetAsync($"{path}"),
            RequestType.Post => await httpClient.PostAsync($"{path}", content),
            RequestType.Put => await httpClient.PutAsync($"{path}", content),
            RequestType.Patch => await httpClient.PatchAsync($"{path}", content),
            RequestType.Delete => await httpClient.DeleteAsync($"{path}"),
            _ => throw new ArgumentNullException(nameof(requestType), "RequestType not found")
        };

        var jsonStr = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<ApiResponse>(jsonStr);
        response ??= new ApiResponse
        {
            Success = false,
            Message = "bad_request",
            Data = ""
        };
        return response;
    }

    private static void SetDefaultHeaders(this HttpClient httpClient)
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