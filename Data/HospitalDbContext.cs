using Microsoft.EntityFrameworkCore;
using Tutorial8.Models;

namespace Tutorial8.Data;

public class HospitalDbContext : DbContext
{
    public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<Admission> Admissions { get; set; }
    public DbSet<Ward> Wards { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Bed> Beds { get; set; }
    public DbSet<BedType> BedTypes { get; set; }
    public DbSet<BedAssignment> BedAssignments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BedAssignment>()
            .Property(b => b.From)
            .HasColumnName("From");

        modelBuilder.Entity<BedAssignment>()
            .Property(b => b.To)
            .HasColumnName("To");
    }
}
