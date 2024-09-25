using CsvHelper;
using ItaliaTreniSharedLibrary.Models;
using System.Globalization;

namespace ItaliaTreniDataImporter
{
    internal class CSVImporter
    {
        public IEnumerable<MeasurementDTO> ReadCsv(string filePath)
        {
            using (var csv = new CsvReader(new StreamReader(filePath), CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<MeasurementDTO>().ToList();
                return records;
            }
        }
    }
}
