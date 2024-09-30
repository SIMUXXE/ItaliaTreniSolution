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
        //Svuota la tabella per fare spazio ai report della nuova elaborazione
        await _context.Database.ExecuteSqlAsync($"TRUNCATE TABLE Defects");

        await AnalyzeMeasurementsAsync();
        await _context.SaveChangesAsync();
        return Ok("Analisi completata e difetti registrati.");
    }

    private async Task AnalyzeMeasurementsAsync()
    {
        var data = await _context.Measurements.ToListAsync();
        var savedDefects = await SearchDefects(data);

        if (savedDefects.Count() > 0)
            _context.Defects.AddRange(savedDefects);
    }

    private async Task<IEnumerable<Defect>> SearchDefects(IEnumerable<Measurement> data)
    {
        List<Defect> defects = new List<Defect>();
        double[] thresholds = { 5.0, 9.0, 10.0 }; // Soglie Arbitrarie scelte: 5.0 - 9.0 - 10.0

        foreach (var measurement in data)
        {
            // Controllo per p1
            CheckAndAddDefect(measurement, measurement.P1, thresholds, "P1", defects);
            // Controllo per p2
            CheckAndAddDefect(measurement, measurement.P2, thresholds, "P2", defects);
            // Controllo per p3
            CheckAndAddDefect(measurement, measurement.P3, thresholds, "P3", defects);
            // Controllo per p4
            CheckAndAddDefect(measurement, measurement.P4, thresholds, "P4", defects);
        }

        return defects;
    }
    private void CheckAndAddDefect(Measurement measurement, double value, double[] thresholds, string parameterName, List<Defect> saveArray)
    {
        Defect defect = null;
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (value > thresholds[i])
            {
                defect = new Defect
                {
                    MeasurementId = measurement.Id,
                    Severity = DetermineSeverity(i),
                    ExceedAmount = value - thresholds[i],
                    Mm = measurement.Mm
                };
            }
        }
        if (defect != null) saveArray.Add(defect);
    }
    private string DetermineSeverity(int index)
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
}