using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("Patients")]
public class Patient
{
    [Key]
    [Column(TypeName = "char(11)")]
    public string Pesel { get; set; } = null!;

    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    public int Age { get; set; }

    public bool Sex { get; set; }

    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
    public ICollection<BedAssignment> BedAssignments { get; set; } = new List<BedAssignment>();
}
