using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiWithRoles.Models.Users;

public class UserRole
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [ForeignKey(nameof(Role))]
    public int RoleId { get; set; }

    public bool IsActive { get; set; }

}