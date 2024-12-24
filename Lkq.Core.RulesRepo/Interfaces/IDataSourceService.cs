using System.Threading.Tasks;
using ERPProcessing;
using Lkq.Models.RulesRepo.CompNine;

namespace Lkq.Core.RulesRepo.Interfaces
{
    public interface IDataSourceService
    {
        CompNineRequestedData GetCompNineData(string vin);
        Task<VINDecodeResponseEntity> GetDataOneData(string vin);
    }
}