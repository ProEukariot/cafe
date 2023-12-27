using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using BCrypt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;

[Route("api/[Controller]")]
[ApiController]
[ValidateModel]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly IUsersData _usersData;

    public AuthController(IConfiguration config, IUsersData usersData)
    {
        _config = config;
        _usersData = usersData;
    }

    // api/Auth/Login
    [HttpPost("Login")]
    public async Task<IResult> Login([FromBody] LoginModel model)
    {
        User? user = null;

        try
        {
            user = await _usersData.GetByUsername(model.Username!);
        }
        catch (SqlException ex) { return Results.BadRequest(ex); }

        if (user == null) return Results.BadRequest();

        bool pwdVerified = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.Hash);

        if (!pwdVerified) return Results.BadRequest();

        var claims = new List<Claim>()
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, user.Role!),
            new Claim(ClaimTypes.DateOfBirth, user.BirthDate.ToString()),
        };

        string token = GenerateToken(claims);

        return Results.Ok(new { token });
    }

    // api/Auth/Registration
    [HttpPost("Registration")]
    public async Task<IResult> Registration([FromBody] RegistrationModel model)
    {
        Guid newInsertedId = Guid.NewGuid();

        try
        {
            await _usersData.Create(new User
            {
                Id = newInsertedId,
                Username = model.Username,
                Hash = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password),
                Email = model.EMail,
                BirthDate = model.BirthDate,
                Role = "Customer"
            });
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627)
                return Results.BadRequest(new { error = "This user exists" });

            return Results.BadRequest(ex);
        }

        var claims = new List<Claim>()
        {
            new Claim("Id", newInsertedId.ToString()),
            new Claim(ClaimTypes.Email, model.EMail!),
            new Claim(ClaimTypes.Role, "Customer"),
            new Claim(ClaimTypes.DateOfBirth, model.BirthDate.ToString()),
        };

        string token = GenerateToken(claims);

        return Results.Ok(new { token });

    }

    // api/Auth/CheckUsers
    [HttpGet("CheckUsers")]
    public async Task<IResult> IfUserExists([FromBody] string? username)
    {
        if (username == null) return Results.BadRequest();

        var userExists = await _usersData.CheckIfExists(username);

        return Results.Ok(userExists);
    }

    [NonAction]
    private string GenerateToken(IEnumerable<Claim>? claims = null)
    {
        JwtSettings jwtSettings = _config
            .GetRequiredSection("AuthenticationSettings")
            .Get<JwtSettings>()!;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey ?? throw new NullReferenceException()));

        JwtSecurityToken tokenOptions = new(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: int.TryParse(jwtSettings.LifeTime, out int seconds) ? DateTime.Now.AddSeconds(seconds) : null,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return token;
    }
}