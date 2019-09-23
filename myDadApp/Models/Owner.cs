using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myDadApp.Models
{
    public class Owner
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime CreateDt { get; set; }
        public Owner()
        {
            Id = Guid.NewGuid().ToString();
            CreateDt = DateTime.UtcNow;
        }
    }
}
