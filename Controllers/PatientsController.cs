using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutorial8.Data;
using Tutorial8.DTOs;

namespace Tutorial8.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly HospitalDbContext _context;

    public PatientsController(HospitalDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPatients([FromQuery] string? search)
    {
        var query = _context.Patients
            .Include(p => p.Admissions)
                .ThenInclude(a => a.Ward)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.BedType)
            .Include(p => p.BedAssignments)
                .ThenInclude(ba => ba.Bed)
                    .ThenInclude(b => b.Room)
                        .ThenInclude(r => r.Ward)
            .AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            search = search.ToLower();
            query = query.Where(p =>
                p.FirstName.ToLower().Contains(search) ||
                p.LastName.ToLower().Contains(search));
        }

        var patients = await query.ToListAsync();

        var result = patients.Select(p => new PatientDto
        {
            Pesel = p.Pesel,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Age = p.Age,
            Sex = p.Sex ? "Male" : "Female",
            Admissions = p.Admissions.Select(a => new AdmissionDto
            {
                Id = a.Id,
                AdmissionDate = a.AdmissionDate,
                DischargeDate = a.DischargeDate,
                Ward = new WardDto
                {
                    Id = a.Ward.Id,
                    Name = a.Ward.Name,
                    Description = a.Ward.Description
                }
            }).ToList(),
            BedAssignments = p.BedAssignments.Select(ba => new BedAssignmentDto
            {
                Id = ba.Id,
                From = ba.From,
                To = ba.To,
                Bed = new BedDto
                {
                    Id = ba.Bed.Id,
                    BedType = new BedTypeDto
                    {
                        Id = ba.Bed.BedType.Id,
                        Name = ba.Bed.BedType.Name,
                        Description = ba.Bed.BedType.Description
                    },
                    Room = new RoomDto
                    {
                        Id = ba.Bed.Room.Id,
                        HasTv = ba.Bed.Room.HasTv,
                        Ward = new WardDto
                        {
                            Id = ba.Bed.Room.Ward.Id,
                            Name = ba.Bed.Room.Ward.Name,
                            Description = ba.Bed.Room.Ward.Description
                        }
                    }
                }
            }).ToList()
        });

        return Ok(result);
    }

    [HttpPost("{pesel}/bedassignments")]
    public async Task<IActionResult> CreateBedAssignment(string pesel, [FromBody] CreateBedAssignmentDto dto)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.Pesel == pesel);
        if (patient == null)
            return NotFound($"Patient with PESEL {pesel} not found");

        var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Name == dto.Ward);
        if (ward == null)
            return NotFound($"Ward '{dto.Ward}' not found");

        var bedType = await _context.BedTypes.FirstOrDefaultAsync(bt => bt.Name == dto.BedType);
        if (bedType == null)
            return NotFound($"Bed type '{dto.BedType}' not found");

        var beds = await _context.Beds
            .Include(b => b.BedAssignments)
            .Where(b => b.Room.WardId == ward.Id && b.BedTypeId == bedType.Id)
            .ToListAsync();

        if (!beds.Any())
            return NotFound($"No beds of type '{dto.BedType}' in ward '{dto.Ward}'");

        var from = dto.From;
        var to = dto.To;

        var availableBed = beds.FirstOrDefault(b =>
            !b.BedAssignments.Any(ba =>
            {
                var baFrom = ba.From;
                var baTo = ba.To;

                if (to == null && baTo == null)
                    return true;
                if (to == null)
                    return baTo > from;
                if (baTo == null)
                    return to > baFrom;

                return baFrom < to && baTo > from;
            })
        );

        if (availableBed == null)
            return NotFound($"No available bed of type '{dto.BedType}' in ward '{dto.Ward}' for the specified period");

        var bedAssignment = new Models.BedAssignment
        {
            PatientPesel = pesel,
            BedId = availableBed.Id,
            From = from,
            To = to
        };

        _context.BedAssignments.Add(bedAssignment);
        await _context.SaveChangesAsync();

        return Created($"/api/patients/{pesel}/bedassignments", new
        {
            bedAssignment.Id,
            bedAssignment.From,
            bedAssignment.To,
            BedId = availableBed.Id,
            PatientPesel = pesel
        });
    }
}
