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

        // Navigational property per la relazione con Measurement
        public virtual Measurement Measurement { get; set; }
    }
}
