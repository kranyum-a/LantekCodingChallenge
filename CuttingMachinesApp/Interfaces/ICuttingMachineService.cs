using CuttingMachinesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CuttingMachinesApp.Interfaces
{
    public interface ICuttingMachineService
    {
        Task<List<CuttingMachine>> GetAllMachinesAsync();
        Task<List<CuttingMachine>> GetMachinesByTechnologyAsync(int technology);
    }
}
