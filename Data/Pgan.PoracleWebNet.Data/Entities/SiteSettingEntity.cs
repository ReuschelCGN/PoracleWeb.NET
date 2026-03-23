using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pgan.PoracleWebNet.Data.Entities;

[Table("site_settings")]
public class SiteSettingEntity
{
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id
    {
        get; set;
    }

    [Column("category")]
    [Required]
    public string Category { get; set; } = string.Empty;

    [Column("key")]
    [Required]
    public string Key { get; set; } = string.Empty;

    [Column("value")]
    public string? Value
    {
        get; set;
    }

    [Column("value_type")]
    [Required]
    public string ValueType { get; set; } = "string";
}
