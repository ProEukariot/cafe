using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

public static class JwtAuthenticationExtension
{
    public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSetttings)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSetttings
                    .Issuer ?? throw new ArgumentNullException(),

                ValidAudience = jwtSetttings
                    .Audience ?? throw new ArgumentNullException(),

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetttings
                    .SecurityKey ?? throw new ArgumentNullException())),
            };
        });
    }
}