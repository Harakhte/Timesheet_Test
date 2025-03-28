using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TImesheet_TEST.Context;
using TImesheet_TEST.Services;

namespace TImesheet_TEST.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context, Timesheet_DbContext dbContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                AttachUserToContext(context, dbContext, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, Timesheet_DbContext dbContext, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                // Attach user to context on successful jwt validation
                var user = dbContext.Users.Find(userId);
                context.Items["User"] = user;

                // Get user roles and attach to context
                var roles = dbContext.UserRoles
                    .Where(ur => ur.UserId == userId)
                    .Join(dbContext.Roles,
                        ur => ur.RoleId,
                        r => r.RoleId,
                        (ur, r) => r.RoleName)
                    .ToList();
                context.Items["Roles"] = roles;
            }
            catch
            {
                // Do nothing if jwt validation fails
                // User is not attached to context so request won't have access to secure routes
            }
        }
    }
}