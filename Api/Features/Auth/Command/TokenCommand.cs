using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Features.Auth.Command;



public class TokenCommand : IRequest<IResult>
{
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public TokenCommand(string? UserName, string? Password)
    {
        this.UserName = UserName;
        this.Password = Password;
    }
};
public class TokenCommandHandler : IRequestHandler<TokenCommand, IResult>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public TokenCommandHandler(UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<IResult> Handle(TokenCommand request, CancellationToken cancellationToken)
    {
        // Verificamos credenciales con Identity
        var user = await _userManager.FindByNameAsync(request.UserName!);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password!))
        {
            return Results.Forbid();
        }

        var roles = await _userManager.GetRolesAsync(user);

        // Generamos un token según los claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(720),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        var tokenResponse = new TokenCommandResponse
        {
            AccessToken = jwt
        };

        return Results.Ok(tokenResponse);
    }
}

public class TokenCommandResponse
{
    public string AccessToken { get; set; } = default!;
}
