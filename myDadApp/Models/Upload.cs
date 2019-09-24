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
        public string Uri { get; set; }
        public DateTime CreateDt { get; set; }

        public Upload()
        {
            PartitionKey = "Demos";
            RowKey = Guid.NewGuid().ToString();
            CreateDt = DateTime.UtcNow;
        }
    }
}
