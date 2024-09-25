using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public async Task<ActionResult<IEnumerable<Measurement>>> GetMeasurements()
    {
        return await _context.Measurements.ToListAsync();
    }

    // POST: api/measurements
    [HttpPost]
    public async Task<ActionResult<Measurement>> PostMeasurement(Measurement measurement)
    {
        var sql = "INSERT INTO Measurements (Mm, P1, P2, P3, P4) VALUES (@Mm, @P1, @P2, @P3, @P4)";

        // Esegui l'inserimento manuale con parametri per evitare SQL Injection
        await _context.Database.ExecuteSqlRawAsync(sql,
            new SqlParameter("@Mm", measurement.Mm),
            new SqlParameter("@P1", measurement.P1),
            new SqlParameter("@P2", measurement.P2),
            new SqlParameter("@P3", measurement.P3),
            new SqlParameter("@P4", measurement.P4)
        );

        // Torna la misura che hai appena aggiunto
        return CreatedAtAction(nameof(GetMeasurements), new { id = measurement.Id }, measurement);
    }
}