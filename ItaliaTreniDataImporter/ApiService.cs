using ItaliaTreniSharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ItaliaTreniDataImporter
{
    internal class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendMeasurementsToApi(IEnumerable<MeasurementDTO> measurements)
        {
            // API URL
            string apiUrl = "https://localhost:7099/api/measure";

            foreach (var measurement in measurements)
            {
                var jsonContent = JsonSerializer.Serialize(measurement);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Errore nell'invio della misura mm: {measurement.Mm}");
                }
            }
        }
    }
}