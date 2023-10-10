using System.Security.Claims;

namespace WebApplication1.Middlewares
{
    public class GetId
    {
        private readonly RequestDelegate _next;

        public GetId(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var NameClaim = context.User.FindFirst("userId");
            if (NameClaim != null)
            {
                context.Items["userId"] = NameClaim.Value;
            }

            await _next(context);
        }
    }
}