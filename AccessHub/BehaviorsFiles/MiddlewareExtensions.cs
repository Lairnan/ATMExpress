namespace AccessHub.BehaviorsFiles;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseHeaderMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HeaderMiddleware>();
    }
}