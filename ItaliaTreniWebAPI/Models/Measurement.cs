using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaTreniSharedLibrary.Models
{
    public class Measurement
    {
        public int Id { get; set; }
        public int Mm { get; set; }
        public double P1 { get; set; }
        public double P2 { get; set; }
        public double P3 { get; set; }
        public double P4 { get; set; }

        public Measurement() { }
        public Measurement(int id, int mm, double p1, double p2, double p3, double p4)
        {
            Id = id;
            Mm = mm;
            P1 = p1;
            P2 = p2;
            P3 = p3;
            P4 = p4;
        }
    }
}