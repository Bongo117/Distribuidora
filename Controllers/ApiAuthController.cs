using Distribuidora.Models; 
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")] 
public class ApiAuthController : ControllerBase
{
    private readonly UserManager<Usuario> _userManager;
    private readonly SignInManager<Usuario> _signInManager;
    private readonly IConfiguration _configuration;


    public ApiAuthController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    [HttpPost("login")] 
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized(new { message = "Email o contraseña inválidos." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

        if (result.Succeeded)
        {
            var token = await GenerateJwtToken(user);
            return Ok(new { token = token }); 
        }

        return Unauthorized(new { message = "Email o contraseña inválidos." });
    }

    private async Task<string> GenerateJwtToken(Usuario user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
        };

        if (!string.IsNullOrEmpty(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }

        
        var userRoles = await _userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("La clave JWT (Jwt:Key) no está configurada en appsettings.json");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        
        var expires = DateTime.Now.AddHours(1);

        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}