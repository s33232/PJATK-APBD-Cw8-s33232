using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("Rooms")]
public class Room
{
    [Key]
    [Column(TypeName = "varchar(4)")]
    public string Id { get; set; } = null!;

    public int WardId { get; set; }

    public bool HasTv { get; set; }

    [ForeignKey("WardId")]
    public Ward Ward { get; set; } = null!;

    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}
