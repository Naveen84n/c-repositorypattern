using System.Collections.Generic;
using System.Threading.Tasks;
using Lkq.Domain.RulesRepo;

namespace Lkq.Data.RulesRepo.Interfaces
{
    public interface IRulesRepository
    {
        Task<IEnumerable<PartTypes>> GetPartTypes();
        Task<IEnumerable<Attributes>> GetAttributes();
        Task<IEnumerable<Attributes>> CheckAttributeAvailablity(string attributeName);
        Task<IEnumerable<AttributeValues>> GetAttributeValues(Attributes attrValues);
        Task<PartRules> SaveRule(PartRules rule);
        Task<IEnumerable<DataSource>> GetDataSources();
        Task<PartRules> GetRule(int id);
        Task<IEnumerable<PartRules>> GetRules(PartRules rule);
        Task<IEnumerable<PartRules>> GetRules(int attributeID, int partType);
        Task<int> GetCount(List<string> partType, bool isContainsAll);
        Task<PartRules> UpdateRule(PartRules rule);
        Task<int> DeleteRule(int ruleid);
    }
}
