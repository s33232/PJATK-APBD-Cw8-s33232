using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("Admissions")]
public class Admission
{
    [Key]
    public int Id { get; set; }

    public DateTime AdmissionDate { get; set; }

    public DateTime? DischargeDate { get; set; }

    [Column(TypeName = "char(11)")]
    public string PatientPesel { get; set; } = null!;

    public int WardId { get; set; }

    [ForeignKey("PatientPesel")]
    public Patient Patient { get; set; } = null!;

    [ForeignKey("WardId")]
    public Ward Ward { get; set; } = null!;
}
