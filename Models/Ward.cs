using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutorial8.Models;

[Table("Wards")]
public class Ward
{
    [Key]
    public int Id { get; set; }

    [MaxLength(300)]
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ICollection<Room> Rooms { get; set; } = new List<Room>();
    public ICollection<Admission> Admissions { get; set; } = new List<Admission>();
}
