using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussMaker.Entities
{
    public class Entity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Fields { get; set; }
    }
}
