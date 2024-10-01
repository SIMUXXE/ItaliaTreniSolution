using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

[Route("api/defects")]
[ApiController]
public class DefectsController : ControllerBase
{
    private readonly ItaliaTreniDbContext _context;

    public DefectsController(ItaliaTreniDbContext context)
    {
        _context = context;
    }

    // GET: api/defects
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Defect>>> GetDefects(int PageNumber, int PageSize)
    {
        return await _context.Defects.OrderByDescending(d => d.ExceedAmount).Skip((PageNumber - 1) * PageSize).Take(PageSize).ToListAsync();
    }

    // POST: api/defects
    [HttpPost]
    public async Task<ActionResult<Defect>> PostDefect(Defect defect)
    {
        var sql = "INSERT INTO Defects (MeasurementId, Severity, ExceedAmount, Mm) VALUES (@Mid, @Sev, @ExAm, @SMm, @EMm);";

        await _context.Database.ExecuteSqlRawAsync(sql,
        new SqlParameter("@Mid", defect.MeasurementId),
        new SqlParameter("@Sev", defect.Severity),
        new SqlParameter("@ExAm", defect.ExceedAmount),
        new SqlParameter("@SMm", defect.StartMm),
        new SqlParameter("@EMm", defect.EndMm)
        );

        return CreatedAtAction(nameof(GetDefects), new { id = defect.Id }, defect);
    }
}
