using Microsoft.AspNetCore.Builder;

namespace MedicalAPI.Middlewares
{
    public static class ErrorLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseGreatErrorHandling(this IApplicationBuilder Builder)
        {
            return Builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}
