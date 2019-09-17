using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myDadApp.Models
{
    public class Upload : TableEntity
    {
        public string Name { get; set; }
        public string URI { get; set; }
        public bool IsCat { get; set; }
        public bool IsDog { get; set; }
        public DateTime CreateDt { get; set; }
        public string Thumbnail { get; set; }
        public Upload()
        {
            PartitionKey = "NewImage";
            RowKey = Guid.NewGuid().ToString();
            CreateDt = DateTime.UtcNow;
            IsCat = IsDog = false;
        }
    }
}
