using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Secu_School_API.Models;


[Table("fine_master")]
public class FineMaster
{
    [Key]
    [Column("fine_id")]
    public int FineId { get; set; }

    [Column("school_id")]
    public int SchoolId { get; set; } = 1;

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("fine_per_day", TypeName = "decimal(10,2)")]
    public decimal FinePerDay { get; set; }

    [Column("grace_period")]
    public int GracePeriod { get; set; } = 0;

    [Column("max_fine", TypeName = "decimal(10,2)")]
    public decimal? MaxFine { get; set; }

    [Column("status")]
    public string Status { get; set; } = "active";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // 🔗 Navigation properties
    public Role? Role { get; set; }
    public School? School { get; set; }
}
