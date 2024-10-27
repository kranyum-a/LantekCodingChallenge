using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingMachinesApp.Model
{
    public class CuttingMachine
    {
        
        public required string Id { get; set; }
        
        public required string Name { get; set; }
        
        public required string Manufacturer { get; set; }
        
        public int Technology { get; set; } 
    }
}
