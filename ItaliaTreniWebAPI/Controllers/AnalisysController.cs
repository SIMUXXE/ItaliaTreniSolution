using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/analisys")]
[ApiController]
public class AnalysisController : ControllerBase
{
    private readonly ItaliaTreniDbContext _context;

    public AnalysisController(ItaliaTreniDbContext context)
    {
        _context = context;
    }

    // POST: api/analysis/analyze
    [HttpPost("analyze")]
    public async Task<IActionResult> Analyze()
    {
        await AnalyzeMeasurementsAsync();
        await _context.SaveChangesAsync();
        return Ok("Analisi completata e difetti registrati.");
    }

    private string GetSeverity(int index)
    {
        switch (index)
        {
            case 0:
                return "Lieve";
            case 1:
                return "Medio";
            case 2:
                return "Grave";
            default:
                return "Unknown";
        }
    }

    private void CheckAndAddDefect(Measurement measurement, double value, double[] thresholds, string parameterName)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value > thresholds[i])
            {
                var defect = new Defect
                {
                    MeasurementId = measurement.Id,
                    Severity = GetSeverity(i),
                    ExceedAmount = value - thresholds[i]
                };
                _context.Defects.Add(defect);
                break;
            }
        }
    }

    private async Task AnalyzeMeasurementsAsync()
    {
        var measurements = await _context.Measurements.ToListAsync();

        foreach (var measurement in measurements)
        {
            // Controllo per p1
            CheckAndAddDefect(measurement, measurement.P1, new[] { 5.0, 9.0, 10.0 }, "P1");
            // Controllo per p2
            CheckAndAddDefect(measurement, measurement.P2, new[] { 100.0, 150.0, 200.0 }, "P2");
            // Controllo per p3
            CheckAndAddDefect(measurement, measurement.P3, new[] { -2.0, -1.0, 0.0 }, "P3");
            // Controllo per p4
            CheckAndAddDefect(measurement, measurement.P4, new[] { -0.5, 0, 0.5 }, "P4");
        }
    }

}
