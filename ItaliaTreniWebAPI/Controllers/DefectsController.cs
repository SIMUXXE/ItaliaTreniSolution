using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public async Task<ActionResult<IEnumerable<Defect>>> GetDefects()
    {
        return await _context.Defects.Include(d => d.Measurement).ToListAsync();
    }

    // POST: api/defects
    [HttpPost]
    public async Task<ActionResult<Defect>> PostDefect(Defect defect)
    {
        _context.Defects.Add(defect);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDefects), new { id = defect.Id }, defect);
    }
}
