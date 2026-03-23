using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pgan.PoracleWebNet.Data.Entities;

[Table("quick_pick_definitions")]
public class QuickPickDefinitionEntity
{
    [Key]
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description
    {
        get; set;
    }

    [Column("icon")]
    [Required]
    public string Icon { get; set; } = "bolt";

    [Column("category")]
    [Required]
    public string Category { get; set; } = "Common";

    [Column("alarm_type")]
    [Required]
    public string AlarmType { get; set; } = "monster";

    [Column("sort_order")]
    public int SortOrder
    {
        get; set;
    }

    [Column("enabled")]
    public bool Enabled { get; set; } = true;

    [Column("scope")]
    [Required]
    public string Scope { get; set; } = "global";

    [Column("owner_user_id")]
    public string? OwnerUserId
    {
        get; set;
    }

    [Column("filters_json")]
    [Required]
    public string FiltersJson { get; set; } = "{}";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
