using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("BedTypes")]
public class BedType
{
    [Key]
    public int Id { get; set; }

    [MaxLength(300)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}
