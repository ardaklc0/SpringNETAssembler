using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussMaker.DataTransferObject.Requests
{
    public class CreateNewEntityRequest
    {
        public string Name { get; set; }
        public string Fields { get; set; }
    }
}
