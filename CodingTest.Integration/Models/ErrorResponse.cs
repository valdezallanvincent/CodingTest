using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTest.Core.Models
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public List<List<ErrorDetail>> Detail { get; set; }
    }
}
