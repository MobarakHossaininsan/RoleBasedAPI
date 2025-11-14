
using System.ComponentModel.DataAnnotations;

namespace ApiWithRoles.Models.Users;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(30)]
    public string UserName { get; set; } = null!;
    
    [StringLength(30)]
    public string Email { get; set; } = null!;
    
    [StringLength(128)]
    public string Password { get; set; } = null!;
    
    [StringLength(16)]
    public string? RegistrationToken { get; set; }
    
    [StringLength(6)]
    public string? PasswordResetPin { get; set; }
    public DateTime? PinExpiryTime { get; set; }
    public bool IsVerified { get; set; }
}