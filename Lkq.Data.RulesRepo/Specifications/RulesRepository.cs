using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Lkq.Data.RulesRepo.DbContexts;
using Lkq.Data.RulesRepo.Interfaces;
using Lkq.Domain.RulesRepo;

namespace Lkq.Data.RulesRepo.Specifications
{

    public class RulesRepository : IRulesRepository
    {
        private readonly PartsDbContext _partsDbContext;
        private readonly AutoCareDbContext _autoCareDbContext;

        public RulesRepository(PartsDbContext partsDbContext, AutoCareDbContext autoCareDbContext)
        {
            _partsDbContext = partsDbContext;
            _autoCareDbContext = autoCareDbContext;
        }

        public async Task<IEnumerable<PartTypes>> GetPartTypes()
        {
            return await _partsDbContext.PartTypes.OrderBy(x => x.PartCodeDescription).ToListAsync();
        }

        public async Task<IEnumerable<Attributes>> GetAttributes()
        {
            return await _partsDbContext.RulesAttributeTables.Where(x => x.AttributeStatus).ToListAsync();
        }

        public async Task<IEnumerable<Attributes>> CheckAttributeAvailablity(string attributeName)
        {
            return await _partsDbContext.RulesAttributeTables.Where(x => x.TableName == attributeName && x.AttributeStatus).ToListAsync();
        }

        public async Task<IEnumerable<AttributeValues>> GetAttributeValues(Attributes attrValues)
        {
            string sqlQuery = "select " + attrValues.KeyName.ToString().Insert(attrValues.KeyName.ToString().Length - 2, "_")
                                + " as AttributeValue_ID," + attrValues.ValueName.ToString()
                                + " as AttributeValue from [AutoCare].tbl_" + attrValues.TableName;
            return await _autoCareDbContext.AttributeValues.FromSqlRaw(sqlQuery).ToListAsync();
        }

        public async Task<PartRules> SaveRule(PartRules rule)
        {
            await _partsDbContext.AddAsync(rule);
            await _partsDbContext.SaveChangesAsync();
            return await GetRule(rule.Part_Rules_ID);

        }

        public async Task<IEnumerable<DataSource>> GetDataSources()
        {
            return await _partsDbContext.DataSources.ToListAsync();
        }


        public async Task<PartRules> GetRule(int id)
        {
            return await _partsDbContext.PartRules.Include(r => r.Attributes).Include(r => r.DataSource)
                .Where(r => r.Part_Rules_ID == id && r.IsActive).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PartRules>> GetRules(int attributeID, int partType)
        {
            List<PartRules> partRules = new();

            if (attributeID == 0 && partType == 0)
            {
                partRules = await _partsDbContext.PartRules.Include(r => r.Attributes).Include(r => r.DataSource)
                    .Where(r => r.IsActive).ToListAsync();
            }
            else if (attributeID != 0 && partType != 0)
            {
                partRules = await _partsDbContext.PartRules.Include(r => r.Attributes).Include(r => r.DataSource)
                    .Where(r => r.IsActive  && r.ACES_Attributes_ID == attributeID
                    && r.PartType == partType.ToString()).ToListAsync();
            }
            else if (attributeID != 0)
            {
                partRules = await _partsDbContext.PartRules.Include(r => r.Attributes).Include(r => r.DataSource)
                    .Where(r => r.IsActive && r.ACES_Attributes_ID == attributeID).ToListAsync();
            }
            else if (partType != 0)
            {
                partRules = await _partsDbContext.PartRules.Include(r => r.Attributes).Include(r => r.DataSource)
                    .Where(r => r.IsActive && r.PartType == partType.ToString()).ToListAsync();
            }

            return partRules;
        }
       
        public async Task<int> GetCount(List<string> partTypes, bool isContainsAll)
        {
            return isContainsAll
                ? await _partsDbContext.PartRules
                    .Where(r => r.IsActive == true).CountAsync()
                : await _partsDbContext.PartRules
                    .Where(r => r.IsActive == true && partTypes.Contains(r.PartType)).CountAsync();
        }
        
        public async Task<PartRules> UpdateRule(PartRules rule)
        {
            var updateRuleResponse = rule;

            _partsDbContext.Entry(await _partsDbContext.PartRules.
                FirstOrDefaultAsync(x => x.Part_Rules_ID == rule.Part_Rules_ID && x.IsActive)).CurrentValues.SetValues(rule);

            if ((await _partsDbContext.SaveChangesAsync()) > 0)
                updateRuleResponse = await GetRule(rule.Part_Rules_ID);

            return updateRuleResponse;
        }

        public async Task<IEnumerable<PartRules>> GetRules(PartRules rule)
        {
            List<PartRules> partRules;

            if (rule.Part_Rules_ID == 0)
                partRules = await _partsDbContext.PartRules.
                Where(x => x.PartType == rule.PartType && x.ACES_Attributes_ID == rule.ACES_Attributes_ID
                && x.AttributeLookup == rule.AttributeLookup && x.Vindecoder_Source_ID == rule.Vindecoder_Source_ID
                && x.IsActive).ToListAsync();
            else
                partRules = await _partsDbContext.PartRules.
                Where(x => x.PartType == rule.PartType && x.ACES_Attributes_ID == rule.ACES_Attributes_ID
                && x.AttributeLookup == rule.AttributeLookup && x.Vindecoder_Source_ID == rule.Vindecoder_Source_ID
                && x.Part_Rules_ID != rule.Part_Rules_ID && x.IsActive ).ToListAsync();

            return partRules;
        }

        public async Task<int> DeleteRule(int ruleid)
        {
            var rule = _partsDbContext.PartRules.Where(x => x.Part_Rules_ID == ruleid && x.IsActive).FirstOrDefault();
            rule.IsActive = false;
            return await _partsDbContext.SaveChangesAsync();
        }
    }
}
