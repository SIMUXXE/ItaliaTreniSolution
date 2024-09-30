using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaTreniSharedLibrary.Models
{
    public class Defect
    {
        public int Id { get; set; }
        public int MeasurementId { get; set; }
        public string Severity { get; set; } 
        public double ExceedAmount { get; set; } 
        public int Mm { get; set; }

        public Defect() { }
        public Defect(int Id, int measurementId, string severity, double exceedAmount, int mm)
        {
            MeasurementId = measurementId;
            Severity = severity;
            ExceedAmount = exceedAmount;
            Mm = mm;
        }
    }
}
