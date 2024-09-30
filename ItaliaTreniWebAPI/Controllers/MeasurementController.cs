using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

[Route("api/measure")]
[ApiController]
public class MeasurementsController : ControllerBase
{
    private readonly ItaliaTreniDbContext _context;

    public MeasurementsController(ItaliaTreniDbContext context)
    {
        _context = context;
    }

    // GET: api/measurements
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements(int PageNumber, int PageSize)
    {
        return await _context.Measurements.OrderBy(m => m.Mm).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
    }

    // POST: api/measurements
    [HttpPost]
    public async Task<ActionResult<Measurement>> PostMeasurement(Measurement measurement)
    {
        var sql = "INSERT INTO Measurements (Mm, P1, P2, P3, P4) VALUES (@Mm, @P1, @P2, @P3, @P4);";

        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Mm", measurement.Mm),
            new SqlParameter("@P1", measurement.P1),
            new SqlParameter("@P2", measurement.P2),
            new SqlParameter("@P3", measurement.P3),
            new SqlParameter("@P4", measurement.P4)
        );

        return CreatedAtAction(nameof(GetMeasurements), new { id = measurement.Id }, measurement);
    }
}