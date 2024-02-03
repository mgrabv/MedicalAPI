using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MedicalAPI.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate Next;

        public ErrorLoggingMiddleware(RequestDelegate Next)
        {
            this.Next = Next;
        }

        public async Task Invoke(HttpContext HttpContext)
        {
            try
            {
                await Next(HttpContext);
            }
            catch (Exception e)
            {
                string ErrorLine = DateTime.Now.ToString() + " --> " + e.Message + "\n";
                await File.AppendAllTextAsync(@"Logs\Log.txt", ErrorLine);

                HttpContext.Response.StatusCode = 500;
                await HttpContext.Response.WriteAsync("Unexpected problem!");
            }
        }
    }
}
