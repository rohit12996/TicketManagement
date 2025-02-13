using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TicketManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace TicketManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly RohitContext _context;

        private readonly IConfiguration _configuration;

        public AuthController(RohitContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
                return BadRequest("Username already exists.");


            if (user.Role == "Admin" || user.Role == "User")
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Role = user.Role;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest("register not successfully Done");
            }


        }
      
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Username == user.Username);
            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
                return Unauthorized("Invalid credentials.");

            // Generate JWT token
            var token = GenerateJwtToken(existingUser);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, user.Role) // Add role claim
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token expiration time
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update-profile/{username}")]
        [Authorize] // Ensure the user is authenticated
        public async Task<IActionResult> UpdateProfile(string username, UpdateUser updateUser_Dto)
        {
            // Check if the user exists
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return NotFound("User  not found.");
            }

            // Update user properties
            user.Username = updateUser_Dto.Username;
            user.Role = updateUser_Dto.Role; // Optional: Update role if provided

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok("User profile updated successfully.");
        }
    }
}
