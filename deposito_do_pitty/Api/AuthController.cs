using deposito_do_pitty.Domain.Entities;
using DepositoDoPitty.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest req)
    {
        var email = (req.Email ?? "").Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(req.Name) || req.Name.Trim().Length < 2)
            return BadRequest(new { message = "Nome inválido." });

        if (string.IsNullOrWhiteSpace(email))
            return BadRequest(new { message = "Email inválido." });

        if (string.IsNullOrWhiteSpace(req.Password) || req.Password.Length < 6)
            return BadRequest(new { message = "Senha deve ter no mínimo 6 caracteres." });

        var exists = await _db.Set<User>().AnyAsync(u => u.Email.ToLower() == email);
        if (exists)
            return Conflict(new { message = "Já existe uma conta com este e-mail." });

        var user = new User
        {
            Name = req.Name.Trim(),
            Email = email,
            Phone = (req.Phone ?? "").Trim(),
            IsActive = true,  
            Role = 0,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(req.Password)
        };

        _db.Set<User>().Add(user);
        await _db.SaveChangesAsync();

        return StatusCode(201, new { user.Id, user.Name, user.Email });
    }
}
