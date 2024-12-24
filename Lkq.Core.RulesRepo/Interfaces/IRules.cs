using System.Collections.Generic;
using System.Threading.Tasks;
using Lkq.Domain.RulesRepo;
using Lkq.Models.RulesRepo;

namespace Lkq.Core.RulesRepo.Interfaces
{
    public interface IRules
    {
        Task<IEnumerable<PartTypesModel>> GetPartTypes();
        Task<IEnumerable<AttributesModel>> GetAttributes();
        Task<IEnumerable<AttributeValuesModel>> GetAttributeValues(string tableName);
        Task<PartRulesModel> SaveRule(RuleRequestModel rule);
        Task<IEnumerable<DataSourceModel>> GetDataSources();
        Task<PartRulesModel> GetRule(int id);
        Task<IEnumerable<PartRulesModel>> GetRules(int attributeID,int partType);
        Task<int> GetCount(List<string> partType);
        Task<PartRulesModel> UpdateRule(int partRulesID, RuleRequestModel rule);
        Task<int> DeleteRule(int partRulesID);
    }
}
