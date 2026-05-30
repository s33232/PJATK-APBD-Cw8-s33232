using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("BedAssignments")]
public class BedAssignment
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "char(11)")]
    public string PatientPesel { get; set; } = null!;

    public int BedId { get; set; }

    public DateTime From { get; set; }

    public DateTime? To { get; set; }

    [ForeignKey("PatientPesel")]
    public Patient Patient { get; set; } = null!;

    [ForeignKey("BedId")]
    public Bed Bed { get; set; } = null!;
}
