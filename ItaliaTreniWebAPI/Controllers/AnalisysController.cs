using ItaliaTreniSharedLibrary.Models;
using ItaliaTreniWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

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

    [HttpPost("customAnalysis")]
    public async Task<IActionResult> CustomAnalysis(int start_Millimeters, int end_Millimeters, double threshold = 0.0)
    {
        List<Defect> res = new List<Defect>();
        List<Measurement> data = await _context.Measurements.ToListAsync();
        for (int i = start_Millimeters; i < end_Millimeters+1; i++)
        {
            if (data[i].P1 > threshold)
                res.Add(new Defect(data[i].Id, "Unknown", (threshold - data[i].P1), data[i].Mm, 0));
            if (data[i].P2 > threshold)
                res.Add(new Defect(data[i].Id, "Unknown", (threshold - data[i].P2), data[i].Mm, 0));
            if (data[i].P3 > threshold)
                res.Add(new Defect(data[i].Id, "Unknown", (threshold - data[i].P3), data[i].Mm, 0));
            if (data[i].P4 > threshold)
                res.Add(new Defect(data[i].Id, "Unknown", (threshold - data[i].P4), data[i].Mm, 0));
        }

        if (res.Count > 0)
        {
            var jsonContent = JsonSerializer.Serialize(res);
            return Ok(jsonContent);
        }
        return Ok("Nessun difetto trovato per i specificati");
    }

    private async Task AnalyzeMeasurementsAsync()
    {
        var savedDefects = await SearchDefects(await _context.Measurements.ToListAsync());

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
                    StartMm = measurement.Mm
                };
            }
        }
        if (defect != null) saveArray.Add(defect);
    }
    private string DetermineSeverity(int index)
    {
        return index switch
        {
            0 => "Lieve",
            1 => "Medio",
            2 => "Grave",
            _ => "Unknown"
        };
    }
}