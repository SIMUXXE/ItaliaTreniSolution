using ItaliaTreniDataImporter;
using ItaliaTreniSharedLibrary.Models;

class Program
{
    static async Task Main(string[] args)
    {
        //CSV records filepath
        string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ff3f3add-7b1d-4f08-ba27-7a9c24fbcd34.csv");

        var importer = new CSVImporter();
        IEnumerable<MeasurementDTO> measurements = importer.ReadCsv(filepath);

        ApiService apiService = new ApiService();

        List<MeasurementDTO> batch = new List<MeasurementDTO>();
        int totalMm = 0;
        int previousMm = 0;

        foreach (var measurement in measurements)
        {
            if (batch.Count == 0)
            {
                previousMm = measurement.Mm;  //Imposta il primo valore "mm" come punto di partenza
            }

            batch.Add(measurement);
            totalMm += (measurement.Mm - previousMm); // Calcola la differenza in mm

            // Se abbiamo raggiunto 1.000.000 mm (1 Km), invia i dati
            if (totalMm >= 1_000_000)
            {
                await apiService.SendMeasurementsToApi(batch);  // Invia 1Km di dati
                batch.Clear();  // Ripulisci il batch
                totalMm = 0;    // Resetta il contatore dei mm
            }

            previousMm = measurement.Mm;  // Aggiorna la posizione precedente
        }

        // Invia eventuali dati rimasti
        if (batch.Count > 0)
        {
            await apiService.SendMeasurementsToApi(batch);
        }
        Console.WriteLine("Invio completato.");
    }
}