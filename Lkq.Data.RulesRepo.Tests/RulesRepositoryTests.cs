using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Lkq.Domain.RulesRepo;
using Lkq.Data.RulesRepo.DbContexts;
using Lkq.Data.RulesRepo.Specifications;

namespace Lkq.Data.RulesRepo.Tests
{
    public class RulesRepositoryTests
    {

        private DbContextOptionsBuilder<PartsDbContext> partsOptions =
           new DbContextOptionsBuilder<PartsDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());
        private DbContextOptionsBuilder<AutoCareDbContext> autoCareOptions =
            new DbContextOptionsBuilder<AutoCareDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString());

        private static List<Attributes> GetTestAttributesInfo()
        {
            List<Attributes> attributeList = new()
            {
                new Attributes
                {
                    ACES_Attributes_ID = 9,
                    TableName = "BedType",
                    KeyName = "BedTypeID",
                    ValueName = "BedTypeName",
                    AttributeStatus = true

                },
                new Attributes
                {
                    ACES_Attributes_ID = 1,
                    TableName = "ApplicationNote2",
                    KeyName = null,
                    ValueName = null,
                    AttributeStatus = false
                },
                new Attributes
                {
                    ACES_Attributes_ID = 10,
                    TableName = "BodyType",
                    KeyName = "BodyTypeID",
                    ValueName = "BodyTypeName",
                    AttributeStatus = true
                }
            };
            return attributeList;
        }


        private static List<PartTypes> GetTestPartTypesInfo()
        {
            List<PartTypes> partTypesList = new()
            {
                new PartTypes
                {
                    Part_Codes_ID = 1129,
                    HollanderPartNo = "5555",
                    PartCode = "CATKP",
                    PartCodeDescription = "1"

                },
                new PartTypes
                {
                    Part_Codes_ID = 404,
                    HollanderPartNo = "1005",
                    PartCode = "APT",
                    PartCodeDescription = "A Panel Trim"

                }
            };
            return partTypesList;
        }

        private static List<AttributeValues> GetTestAttributeValuesInfo()
        {
            List<AttributeValues> attributeValues = new()
            {
                new AttributeValues
                {
                    AttributeValue_ID = 2,
                    AttributeValue = "N/A"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 3,
                    AttributeValue = "N/R"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 5,
                    AttributeValue = "Fleetside"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 10,
                    AttributeValue = "Stepside"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 14,
                    AttributeValue = "U/K"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 16,
                    AttributeValue = "Stakebed"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 17,
                    AttributeValue = "Fold-Out Side Walls"
                },
                new AttributeValues
                {
                    AttributeValue_ID = 19,
                    AttributeValue = "Box"
                }
            };
            return attributeValues;
        }

        private static List<PartRules> GetTestRulesInfo()
        {
            List<PartRules> rulesList = new()
            {
                new PartRules
                {
                    Part_Rules_ID = 19,
                    PartType = "300",
                    ACES_Attributes_ID = 1,
                    AttributeLookup = "10",
                    Vindecoder_Source_ID = 1,
                    PropertyPath = "query_responses.common_data.basic_data.body_type",
                    PropertyValue = "Convertible",
                    Ordinal = 13,
                    IsActive = true,
                    RulesDescription = null,

                },
                new PartRules
                {
                    Part_Rules_ID = 20,
                    PartType = "300",
                    ACES_Attributes_ID = 1,
                    AttributeLookup = "22",
                    Vindecoder_Source_ID = 1,
                    PropertyPath = "query_responses.common_data.basic_data.body_type",
                    PropertyValue = "Crew Cab",
                    Ordinal = 13,
                    IsActive = true,
                    RulesDescription = null,
                }
            };
            return rulesList;
        }

        private static PartRules GetTestRuleInfo()
        {
            var rule = new PartRules
            {
                Part_Rules_ID = 19,
                PartType = "300",
                ACES_Attributes_ID = 10,
                AttributeLookup = "10",
                Vindecoder_Source_ID = 2,
                PropertyPath = "query_responses.common_data.basic_data.body_type",
                PropertyValue = "Convertible",
                Ordinal = 13,
                IsActive = true,
                RulesDescription = null,
                DataSource = new DataSource
                {
                    Vindecoder_Source_ID = 2,
                    Name = "DataOne"
                },
                Attributes = new Attributes
                {
                    ACES_Attributes_ID = 10,
                    TableName = "BodyType",
                    KeyName = "BodyTypeID",
                    ValueName = "BodyTypeName",
                    AttributeStatus = true
                }
            };


            return rule;
        }

        private static List<DataSource> GetTestDataSources()
        {
            List<DataSource> dataSources = new()
            {
                new DataSource
                {
                    Vindecoder_Source_ID = 1,
                    Name = "CompNine"
                },
                new DataSource
                {
                    Vindecoder_Source_ID = 2,
                    Name = "DataOne"
                }
            };

            return dataSources;
        }

        [Fact]
        public async Task GetAttributes_ReturnsListOfAttributes()
        {
            //Arrange
            var result = GetTestAttributesInfo();
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            foreach (Attributes attributeList in result)
            {
                partsDbcontext.RulesAttributeTables.Add(attributeList);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.GetAttributes();

            // Assert         
            Assert.Equal(2, response.Count());

        }

        [Fact]
        public async Task GetPartTypes_ReturnsListOfPartTypes()
        {
            //Arrange
            var result = GetTestPartTypesInfo();
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            foreach (PartTypes partTypesList in result)
            {
                partsDbcontext.PartTypes.Add(partTypesList);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.GetPartTypes();

            // Assert         
            Assert.Equal(2, response.Count());

        }

        [Fact]
        public async Task GetChkAttribute_ReturnsListOfAttribute()
        {
            //Arrange
            var attributesResult = GetTestAttributesInfo();
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            foreach (Attributes attributes in attributesResult)
            {
                partsDbcontext.RulesAttributeTables.Add(attributes);
            }
            partsDbcontext.SaveChanges();

            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.CheckAttributeAvailablity("BedType");

            // Assert         
            Assert.Single(response);
        }

        [Fact]
        public async Task GetAttributeValues_ReturnsListOfAttributeValues()
        {
            //Arrange
            Attributes attrValuesParameter = new()
            {
                ACES_Attributes_ID = 9,
                TableName = "BedType",
                KeyName = "BedTypeID",
                ValueName = "BedTypeName",
                AttributeStatus = true
            };
            var attributeValuesResult = GetTestAttributeValuesInfo();
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            foreach (AttributeValues attributeValues in attributeValuesResult)
            {
                autoCareDbcontext.AttributeValues.Add(attributeValues);
            }
            autoCareDbcontext.SaveChanges();

            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.GetAttributeValues(attrValuesParameter);

            // Assert         
            Assert.Equal(8, response.Count());
        }

        [Fact]
        public async Task GetRule_PartRulesID_ReturnsRule()
        {
            //Arrange
            int id = 19;
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();
            var DataSource = new DataSource
            {
                Name = "DataOne"
            };
            var Attributes = new Attributes
            {
                TableName = "BodyType",
                KeyName = "BodyTypeID",
                ValueName = "BodyTypeName",
                AttributeStatus = true
            };

            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.RulesAttributeTables.Add(Attributes);
            partsDbcontext.DataSources.Add(DataSource);
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var ruleResponse = await repo.GetRule(id);

            // Assert
            Assert.Equal(19, ruleResponse.Part_Rules_ID);
        }

        [Fact]
        public async Task GetDataSources_ReturnsListOfDataSources()
        {
            //Arrange
            var result = GetTestDataSources();
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            foreach (DataSource dataSource in result)
            {
                partsDbcontext.DataSources.Add(dataSource);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.GetDataSources();

            // Assert         
            Assert.Equal(2, response.Count());
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 300)]
        [InlineData(1, 0)]
        [InlineData(0, 300)]
        public async Task GetRules_ValidInput_ReturnsListOfRule(int attributeID, int partType)
        {
            //Arrange
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();
            var DataSource = new DataSource
            {
                Name = "DataOne"
            };
            var Attributes = new Attributes
            {
                TableName = "BodyType",
                KeyName = "BodyTypeID",
                ValueName = "BodyTypeName",
                AttributeStatus = true
            };

            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.DataSources.Add(DataSource);
            partsDbcontext.RulesAttributeTables.Add(Attributes);
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var ruleResponse = await repo.GetRules(attributeID, partType);

            // Assert   
            Assert.Equal(2, ruleResponse.Count());
        }

        [Fact]
        public async Task GetRules_InvalidInput_ReturnsEmpty()
        {
            //Arrange
            int id = 16;
            int partType = 0;
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);


            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var ruleResponse = await repo.GetRules(id, partType);

            // Assert  
            Assert.Empty(ruleResponse);
        }

        [Fact]
        public async Task SaveRule_ValidPartRules_ReturnsPartRules()
        {
            //Arrange
            PartRules partRules = new()
            {
                PartType = "200",
                ACES_Attributes_ID = 1,
                AttributeLookup = "10",
                Vindecoder_Source_ID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Code",
                PropertyValue = "TRDE",
                Ordinal = 5,
                CreatedDate = DateTime.Now,
                CreatedUser = "Admin",
                IsActive = true,
                RulesDescription = "RuleDesc"

            };
            var DataSource = new DataSource
            {
                Name = "DataOne"
            };
            var Attributes = new Attributes
            {
                TableName = "BodyType",
                KeyName = "BodyTypeID",
                ValueName = "BodyTypeName",
                AttributeStatus = true
            };

            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            partsDbcontext.PartRules.Add(GetTestRuleInfo());
            partsDbcontext.DataSources.Add(DataSource);
            partsDbcontext.RulesAttributeTables.Add(Attributes);
            partsDbcontext.SaveChanges();

            //Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.SaveRule(partRules);

            // Assert         
            Assert.Equal(20, response.Part_Rules_ID);
        }

        [Fact]
        public async Task SaveRule_InvalidPartRules_ReturnsNull()
        {
            //Arrange
            PartRules partRules = new()
            {
                PartType = "300",
                ACES_Attributes_ID = 10,
                AttributeLookup = "10",
                Vindecoder_Source_ID = 2,
                PropertyPath = "RequestedData.CompNineData.OptionList.Code",
                PropertyValue = "TRDE",
                Ordinal = 5,
                CreatedDate = DateTime.Now,
                CreatedUser = "Admin",
                IsActive = true,
                RulesDescription = "RuleDesc"

            };

            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);

            //Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.SaveRule(partRules);

            // Assert         
            Assert.Null(response);
        }

        [Fact]
        public async Task UpdateRule_ValidPartRules_ReturnsPartRules()
        {
            //Arrange
            PartRules partRules = new()
            {
                Part_Rules_ID = 19,
                PartType = "300",
                ACES_Attributes_ID = 10,
                AttributeLookup = "10",
                Vindecoder_Source_ID = 2,
                PropertyPath = "RequestedData.CompNineData.OptionList.Code",
                PropertyValue = "TRDE",
                Ordinal = 5,
                CreatedDate = DateTime.Now,
                CreatedUser = "Admin",
                IsActive = true,
                RulesDescription = "RuleDesc"

            };
            var DataSource = new DataSource
            {
                Name = "DataOne"
            };
            var Attributes = new Attributes
            {
                TableName = "BodyType",
                KeyName = "BodyTypeID",
                ValueName = "BodyTypeName",
                AttributeStatus = true
            };

            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            partsDbcontext.PartRules.Add(GetTestRuleInfo());
            partsDbcontext.DataSources.Add(DataSource);
            partsDbcontext.RulesAttributeTables.Add(Attributes);
            partsDbcontext.SaveChanges();

            //Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.UpdateRule(partRules);

            // Assert         
            Assert.Equal(19, response.Part_Rules_ID);
            Assert.True(response.IsActive);
        }


        [Fact]
        public async Task UpdateRule_InvalidPartRules_ReturnsNull()
        {
            //Arrange
            PartRules partRules = new()
            {
                Part_Rules_ID = 19,
                PartType = "200",
                ACES_Attributes_ID = 2,
                AttributeLookup = "751",
                Vindecoder_Source_ID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                ModifiedDate = DateTime.Now,
                ModifiedUser = "Admin",
                RulesDescription = "RuleDesc"

            };

            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            partsDbcontext.PartRules.Add(GetTestRuleInfo());
            partsDbcontext.SaveChanges();

            //Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var response = await repo.UpdateRule(partRules);

            // Assert         
            Assert.Null(response);

        }

        [Fact]
        public async Task DeleteRule_ValidRulesID_ReturnsDeletedRowCount()
        {
            //Arrange
            int id = 19;
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();
            var DataSource = new DataSource
            {
                Name = "DataOne"
            };
            var Attributes = new Attributes
            {
                TableName = "BodyType",
                KeyName = "BodyTypeID",
                ValueName = "BodyTypeName",
                AttributeStatus = true
            };

            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.RulesAttributeTables.Add(Attributes);
            partsDbcontext.DataSources.Add(DataSource);
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var ruleResponse = await repo.DeleteRule(id);

            // Assert
            Assert.Equal(1, ruleResponse);
        }


        [Fact]
        public async Task GetCount_PartTypesList_ReturnsCount()
        {
            //Arrange
            List<string> partType = new List<string> { "300" };
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();
          
            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var count = await repo.GetCount(partType, false);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCount_PartTypesAll_ReturnsCount()
        {
            //Arrange
            List<string> partType = new List<string> { "ALL" };
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();
            
            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            var count = await repo.GetCount(partType, true);

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCount_PartTypesList_ReturnsCountZero()
        {
            //Arrange
            List<string> partType = new List<string> { "1000" };
            var partsDbcontext = new PartsDbContext(partsOptions.Options);
            var autoCareDbcontext = new AutoCareDbContext(autoCareOptions.Options);
            var rules = GetTestRulesInfo();

            foreach (PartRules rule in rules)
            {
                partsDbcontext.PartRules.Add(rule);
            }
            partsDbcontext.SaveChanges();
            // Act
            var repo = new RulesRepository(partsDbcontext, autoCareDbcontext);
            int count = await repo.GetCount(partType,false);

            // Assert
            Assert.Equal(0, count);
        }

    }
}
