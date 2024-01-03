using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussMaker.DataTransferObject.Responses
{
    public class GetEntityDisplayResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fields { get; set; }
    }
}
