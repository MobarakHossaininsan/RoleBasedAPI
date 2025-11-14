using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiWithRoles.Data;
using ApiWithRoles.DTOs;
using ApiWithRoles.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ApiWithRoles.Services;

public class UserServices
{
        private readonly AppDbContext _context;
        private readonly PasswordHasher<User> _hasher = new();
        private readonly IConfiguration _config;

        public UserServices(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        // ================= REGISTER =================
        public async Task<string> RegisterUserAsync(Register request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
                return "Email already registered.";

            var code = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                UserName = request.Username,
                Email = request.Email,
                Password = string.Empty,
                PasswordResetPin = code,
                PinExpiryTime = DateTime.UtcNow.AddMinutes(5),
                IsVerified = false
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SendEmailAsync(user.Email, "Your Verification Code",
                $"Hello {user.UserName},\n\nYour verification code is: {code}\nThis code will expire in 5 minutes.");

            return "Verification code sent to your email.";
        }

        // ================= VERIFY =================
        public async Task<string> VerifyUserAsync(Verify request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null) return "User not found.";
            if (user.IsVerified) return "User already verified.";
            if (user.PinExpiryTime < DateTime.UtcNow)
                return "Verification code expired. Please register again.";
            if (user.PasswordResetPin != request.Code)
                return "Invalid code.";

            user.IsVerified = true;
            user.PasswordResetPin = null;
            user.PinExpiryTime = null;

            await _context.SaveChangesAsync();
            return "Email verified successfully!";
        }

        // ================= SET PASSWORD =================
        public async Task<string> SetPasswordAsync(SetPassword request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (user == null) return "User not found.";
            if (!user.IsVerified) return "User not verified yet.";

            user.Password = _hasher.HashPassword(user, request.Password);
            await _context.SaveChangesAsync();

            return "Password set successfully!";
        }

        // ================= LOGIN =================
        public string LoginUser(Login request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null) return "User not found.";
            if (!user.IsVerified) return "User not verified yet.";

            var result = _hasher.VerifyHashedPassword(user, user.Password, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return "Invalid password.";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }

        // ================= EMAIL SENDER =================
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new System.Net.NetworkCredential("mobarakhossaininsan17@gmail.com", "esvbeedgdlihpkxf"),
                EnableSsl = true
            };

            var message = new System.Net.Mail.MailMessage("your_email@gmail.com", toEmail, subject, body);
            await smtp.SendMailAsync(message);
        }
}