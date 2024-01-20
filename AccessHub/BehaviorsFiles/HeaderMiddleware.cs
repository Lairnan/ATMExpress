using Configuration;

namespace AccessHub.BehaviorsFiles;

public class HeaderMiddleware
{
    private readonly RequestDelegate _next;
    public HeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("language", out var language) && Enum.TryParse(language, out Languages lang))
            Translate.SetLanguage(lang);
        else Translate.SetLanguage(Languages.En);

        await _next(context);
    }
}