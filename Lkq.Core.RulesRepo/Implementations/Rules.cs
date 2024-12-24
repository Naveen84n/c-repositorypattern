using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lkq.Core.RulesRepo.Common;
using Lkq.Core.RulesRepo.Interfaces;
using Lkq.Data.RulesRepo.Interfaces;
using Lkq.Domain.RulesRepo;
using Lkq.Models.RulesRepo;
using Microsoft.Extensions.Configuration;

namespace Lkq.Core.RulesRepo.Implementations
{
    public class Rules : IRules
    {
        private readonly IMapper _mapper;
        private readonly IRulesRepository _rulesRepository;
        private readonly IConfiguration _config;

        public Rules(IMapper mapper, IRulesRepository rulesRepository, IConfiguration config)
        {
            _mapper = mapper;
            _rulesRepository = rulesRepository;
            _config = config;
        }

        public async Task<IEnumerable<PartTypesModel>> GetPartTypes()
        {
            var partTypes = await _rulesRepository.GetPartTypes();
            return _mapper.Map<List<PartTypes>, List<PartTypesModel>>(partTypes.ToList());
        }

        public async Task<IEnumerable<AttributesModel>> GetAttributes()
        {
            var attributeTables = await _rulesRepository.GetAttributes();
            var attributeTablesList = _mapper.Map<List<Attributes>, List<AttributesModel>>(attributeTables.ToList());
            return attributeTablesList;

        }


        public async Task<IEnumerable<AttributeValuesModel>> GetAttributeValues(string attributeName)
        {
            var attribute = await _rulesRepository.CheckAttributeAvailablity(attributeName);
            var attributeValuesParam = attribute.FirstOrDefault();
            if (!attribute.Any())
                return new List<AttributeValuesModel>();

            var attributeValues = await _rulesRepository.GetAttributeValues(attributeValuesParam);
            return _mapper.Map<List<AttributeValues>, List<AttributeValuesModel>>(attributeValues.ToList());
        }

        public async Task<PartRulesModel> SaveRule(RuleRequestModel rule)
        {

            var saveRequest = _mapper.Map<RuleRequestModel, PartRules>(rule);
            var response = saveRequest;

            var result = await _rulesRepository.GetRules(saveRequest);
            if (!result.Any())
            {
                saveRequest.CreatedUser = rule.User;
                saveRequest.IsActive = true;
                saveRequest.CreatedDate = DateTime.Now;
                response = await _rulesRepository.SaveRule(saveRequest);
            }

            return _mapper.Map<PartRules, PartRulesModel>(response);
        }

        public async Task<IEnumerable<DataSourceModel>> GetDataSources()
        {
            var dataSources = await _rulesRepository.GetDataSources();
            var response = dataSources.Select(kv => new DataSourceModel(kv.Vindecoder_Source_ID, kv.Name)).ToList();
            foreach (DataSourceModel dataSource in response)
            {
                var classType = _config.GetSection("DataStructure").GetSection(dataSource.DataSource).Value;
                dataSource.Structure = StructureHelper.ExtractStructure(Type.GetType(classType));
            }
            return response;
        }

        public async Task<PartRulesModel> GetRule(int id)
        {
            var rule = await _rulesRepository.GetRule(id);
            return _mapper.Map<PartRules, PartRulesModel>(rule);
        }

        public async Task<IEnumerable<PartRulesModel>> GetRules(int attributeID, int partType)
        {
            var rules = await _rulesRepository.GetRules(attributeID, partType);
            return _mapper.Map<List<PartRules>, List<PartRulesModel>>(rules.ToList());
        }

        public async Task<int> GetCount(List<string> partType)
        {
            bool isContainsAll = partType.Any(x => string.Compare(x, "All", StringComparison.InvariantCultureIgnoreCase) == 0);
            return await _rulesRepository.GetCount(partType, isContainsAll);
        }
        public async Task<PartRulesModel> UpdateRule(int partRulesID, RuleRequestModel rule)
        {
            PartRules response = null;

            var updatRequest = _mapper.Map<RuleRequestModel, PartRules>(rule);
            updatRequest.Part_Rules_ID = partRulesID;

            var existingRule = await _rulesRepository.GetRule(partRulesID);

            if (existingRule != null)
            {
                var duplicateCheck = await _rulesRepository.GetRules(updatRequest);

                if (!duplicateCheck.Any())
                {
                    updatRequest.ModifiedDate = DateTime.Now;
                    updatRequest.CreatedDate = existingRule.CreatedDate;
                    updatRequest.CreatedUser = existingRule.CreatedUser;
                    updatRequest.ModifiedUser = rule.User;
                    updatRequest.IsActive = true;
                    response = await _rulesRepository.UpdateRule(updatRequest);
                }
                else
                {
                    response = updatRequest;
                }
            }

            return response != null ? _mapper.Map<PartRules, PartRulesModel>(response) : null;
        }

        public async Task<int> DeleteRule(int partRulesID)
        {
            int deleteRuleResponse = 0;
            var existingRule = await _rulesRepository.GetRule(partRulesID);
            if (existingRule != null)
                deleteRuleResponse = await _rulesRepository.DeleteRule(partRulesID);
            
            return deleteRuleResponse;
        }
    }
}
