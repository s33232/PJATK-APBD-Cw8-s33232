using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("Beds")]
public class Bed
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Column(TypeName = "varchar(4)")]
    public string RoomId { get; set; } = null!;

    public int BedTypeId { get; set; }

    [ForeignKey("RoomId")]
    public Room Room { get; set; } = null!;

    [ForeignKey("BedTypeId")]
    public BedType BedType { get; set; } = null!;

    public ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();
}
