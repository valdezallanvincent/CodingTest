using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Core.Models
{
    public class RainfallApiItem
    {
        public string EaRegionName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public RainfallMeasure Measures { get; set; }
    }
}
