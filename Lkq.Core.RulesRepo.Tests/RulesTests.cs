using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using AutoMapper;
using Lkq.Models.RulesRepo;
using Lkq.Core.RulesRepo.Implementations;
using Lkq.Core.RulesRepo.MappingProfiles;
using Lkq.Data.RulesRepo.Interfaces;
using Lkq.Domain.RulesRepo;
using Microsoft.Extensions.Configuration;

namespace Lkq.Core.RulesRepo.Tests
{
    public class RulesTests
    {
        private static IMapper _mapper;
        public static IMapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    // Auto Mapper Configurations
                    var mappingConfig = new MapperConfiguration(mc =>
                    {
                        mc.AddProfile(new MappingProfile());
                    });
                    IMapper mapper = mappingConfig.CreateMapper();
                    _mapper = mapper;
                }
                return _mapper;
            }
        }
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

        private static int RulesCount()
        {
            int count = 2;
            return count;
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
        private static List<PartRules> GetTestRulesInfo()
        {
            List<PartRules> rulesList = new()
            {
                new PartRules
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
                    RulesDescription = null
                },
                new PartRules
                {
                    Part_Rules_ID = 20,
                    PartType = "300",
                    ACES_Attributes_ID = 10,
                    AttributeLookup = "22",
                    Vindecoder_Source_ID = 2,
                    PropertyPath = "query_responses.common_data.basic_data.body_type",
                    PropertyValue = "Crew Cab",
                    Ordinal = 13,
                    IsActive = true,
                    RulesDescription = null
                }
            };
            return rulesList;
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

            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetAttributes())
                .ReturnsAsync(GetTestAttributesInfo());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetAttributes();

            //Assert
            Assert.IsType<List<AttributesModel>>(response);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task GetPartTypes_ReturnsListOfPartTypes()
        {
            //Arrange
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetPartTypes())
                .ReturnsAsync(GetTestPartTypesInfo());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetPartTypes();

            //Assert
            Assert.IsType<List<PartTypesModel>>(response);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task GetRule_ValidPartRulesID_ReturnsListOfRule()
        {
            //Arrange
            int id = 19;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetRule(id)).ReturnsAsync(GetTestRuleInfo());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetRule(id);

            //Assert
            Assert.IsType<PartRulesModel>(response);
            Assert.Equal(19, response.RulesID);
        }

        [Fact]
        public async Task GetRule_InvalidPartRulesID_ReturnsEmpty()
        {
            //Arrange
            int id = 2;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetRule(id)).ReturnsAsync(value: null);
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetRule(id);

            //Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task GetRules_ValidInput_ReturnsListOfRules()
        {
            //Arrange
            int attributeID = 10;
            int partType = 300;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetRules(attributeID, partType))
                .ReturnsAsync(GetTestRulesInfo());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetRules(attributeID, partType);

            //Assert
            Assert.IsType<List<PartRulesModel>>(response);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task GetRules_InvalidInput_ReturnsEmpty()
        {
            //Arrange
            int attributeID = 1;
            int partType = 99;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetRules(attributeID, partType))
                .ReturnsAsync(new List<PartRules>());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetRules(attributeID, partType);

            //Assert
            Assert.IsType<List<PartRulesModel>>(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetAttributeValues_ValidAttributeName_ReturnsListofAttributeValues()
        {
            //Arrange
            string attributeValue = "BedType";
            Attributes attrValuesParameter = new()
            {
                ACES_Attributes_ID = 9,
                TableName = "BedType",
                KeyName = "BedTypeID",
                ValueName = "BedTypeName",
                AttributeStatus = true
            };
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();

            _mockRepository.Setup(Pr => Pr.CheckAttributeAvailablity(attributeValue))
                .ReturnsAsync(GetTestAttributesInfo());

            _mockRepository.Setup(Pr => Pr.GetAttributeValues(It.IsAny<Attributes>()))
                .ReturnsAsync(GetTestAttributeValuesInfo());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetAttributeValues(attributeValue);

            //Assert
            Assert.IsType<List<AttributeValuesModel>>(response);
            Assert.Equal(8, response.Count());
        }

        [Fact]
        public async Task SaveRule_ValidRequest_ReturnsCreatedData()
        {
            //Arrange
            var _mockRepository = new Mock<IRulesRepository>();
            var _mockConfig = new Mock<IConfiguration>();
            var request = new RuleRequestModel
            {
                PartType = "400",
                AttributesID = 21,
                AttributeLookup = "751",
                DataSourceID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                User = "Admin",
                RulesDescription = "RuleDesc"
            };

            _mockRepository.Setup(Pr => Pr.SaveRule(It.IsAny<PartRules>()))
                .ReturnsAsync(GetTestRuleInfo());

            _mockRepository.Setup(Pr => Pr.GetRules(It.IsAny<PartRules>()))
                .ReturnsAsync(new List<PartRules>());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            var response = await service.SaveRule(request);

            //Assert
            Assert.IsType<PartRulesModel>(response);
            Assert.Equal(19, response.RulesID);
        }

        [Fact]
        public async Task SaveRule_DuplicateRequest_ReturnsRulesIDZero()
        {
            //Arrange
            var _mockRepository = new Mock<IRulesRepository>();
            var _mockConfig = new Mock<IConfiguration>();
            var request = new RuleRequestModel
            {
                PartType = "400",
                AttributesID = 21,
                AttributeLookup = "751",
                DataSourceID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                User = "Admin",
                RulesDescription = "RuleDesc"
            };

            _mockRepository.Setup(Pr => Pr.SaveRule(It.IsAny<PartRules>()))
                .ReturnsAsync(new PartRules());

            _mockRepository.Setup(Pr => Pr.GetRules(It.IsAny<PartRules>()))
              .ReturnsAsync(GetTestRulesInfo());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            var response = await service.SaveRule(request);

            //Assert
            Assert.IsType<PartRulesModel>(response);
            Assert.Equal(0, response.RulesID);

        }

        [Fact]
        public async Task GetAttributeValues_InvalidAttributeName_ReturnsEmpty()
        {
            //Arrange
            string attributeValue = "BedType";
            Attributes attrValuesParameter = new()
            {
                ACES_Attributes_ID = 9,
                TableName = "BedType",
                KeyName = "BedTypeID",
                ValueName = "BedTypeName",
                AttributeStatus = true
            };
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();

            _mockRepository.Setup(Pr => Pr.CheckAttributeAvailablity(attributeValue))
                .ReturnsAsync(GetTestAttributesInfo());

            _mockRepository.Setup(Pr => Pr.GetAttributeValues(It.IsAny<Attributes>()))
                .ReturnsAsync(new List<AttributeValues>());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetAttributeValues(attributeValue);

            //Assert
            Assert.IsType<List<AttributeValuesModel>>(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetDataSources_ReturnsListOfDataSources()
        {
            //Arrange
            string compNinePath = "Lkq.Models.RulesRepo.CompNine.CompNineRequestedData, Lkq.Models.RulesRepo";
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(r => r.GetDataSources()).ReturnsAsync(GetTestDataSources());

            _mockConfig.Setup(a => a.GetSection(It.IsAny<string>()).GetSection(It.IsAny<string>()).Value)
                .Returns(compNinePath);

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.GetDataSources();

            //Assert
            Assert.IsType<List<DataSourceModel>>(response);
            Assert.Equal(2, response.Count());
        }

        [Fact]
        public async Task GetCount_ValidPartTypes_ReturnsRulesCount()
        {
            //Arrange
            List<string> partType = new List<string> { "300" };
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetCount(partType,false)).ReturnsAsync(RulesCount());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            int count = await service.GetCount(partType);

            //Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetCount_ValidPartTypesAll_ReturnsZeroCount()
        {
            //Arrange
            List<string> partType = new List<string> { "02" };
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.GetCount(partType, true)).ReturnsAsync(value:0);
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            int count = await service.GetCount(partType);

            //Assert
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task UpdateRule_ValidRequest_ReturnsUpdatedData()
        {
            //Arrange
            var _mockRepository = new Mock<IRulesRepository>();
            var _mockConfig = new Mock<IConfiguration>();
            var request = new RuleRequestModel
            {
                PartType = "400",
                AttributesID = 21,
                AttributeLookup = "751",
                DataSourceID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                User = "Admin",
                RulesDescription = "RuleDesc"
            };

            var id = 19;

            _mockRepository.Setup(Pr => Pr.UpdateRule(It.IsAny<PartRules>()))
                .ReturnsAsync(GetTestRuleInfo());
            _mockRepository.Setup(Pr => Pr.GetRule(It.IsAny<int>()))
                .ReturnsAsync(GetTestRuleInfo());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            var response = await service.UpdateRule(id, request);

            //Assert
            Assert.IsType<PartRulesModel>(response);
            Assert.True(response.IsActive);
        }

        [Fact]
        public async Task UpdateRule_DuplicateRequest_ReturnsIsActiveFalse()
        {
            //Arrange
            var _mockRepository = new Mock<IRulesRepository>();
            var _mockConfig = new Mock<IConfiguration>();
            var request = new RuleRequestModel
            {
                PartType = "300",
                AttributesID = 10,
                AttributeLookup = "10",
                DataSourceID = 2,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                User = "Admin",
                RulesDescription = "RuleDesc"
            };
            var id = 297;

            _mockRepository.Setup(Pr => Pr.UpdateRule(It.IsAny<PartRules>()))
                .ReturnsAsync(new PartRules { IsActive = false });

            _mockRepository.Setup(Pr => Pr.GetRule(It.IsAny<int>()))
               .ReturnsAsync(new PartRules { IsActive = false });

            _mockRepository.Setup(Pr => Pr.GetRules(It.IsAny<PartRules>()))
               .ReturnsAsync(GetTestRulesInfo());

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            var response = await service.UpdateRule(id, request);

            //Assert
            Assert.IsType<PartRulesModel>(response);
            Assert.False(response.IsActive);

        }

        [Fact]
        public async Task UpdateRule_InValidRequest_ReturnsNull()
        {
            //Arrange
            var _mockRepository = new Mock<IRulesRepository>();
            var _mockConfig = new Mock<IConfiguration>();
            var request = new RuleRequestModel
            {
                PartType = "400",
                AttributesID = 21,
                AttributeLookup = "751",
                DataSourceID = 1,
                PropertyPath = "RequestedData.CompNineData.OptionList.Value",
                PropertyValue = "4R70W",
                Ordinal = 5,
                User = "Admin",
                RulesDescription = "RuleDesc"
            };
            var id = 1500;

            _mockRepository.Setup(Pr => Pr.UpdateRule(It.IsAny<PartRules>()));

            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            var response = await service.UpdateRule(id, request);

            //Assert
            Assert.Null(response);

        }

        [Fact]
        public async Task DeleteRule_ValidRequest_ReturnsDeletedRowCount()
        {
            //Arrange
            int id = 297;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.DeleteRule(id)).ReturnsAsync(value: 1);
            _mockRepository.Setup(result => result.GetRule(id)).ReturnsAsync(GetTestRuleInfo());
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.DeleteRule(id);

            //Assert
            Assert.IsType<int>(response);
            Assert.Equal(1, response);
        }

        [Fact]
        public async Task DeleteRule_InvalidPartRulesID_ReturnsZero()
        {
            //Arrange
            int id = 297;
            var _mockConfig = new Mock<IConfiguration>();
            var _mockRepository = new Mock<IRulesRepository>();
            _mockRepository.Setup(result => result.DeleteRule(id)).ReturnsAsync(value: 0);
            var service = new Rules(Mapper, _mockRepository.Object, _mockConfig.Object);

            //Act
            var response = await service.DeleteRule(id);

            //Assert
            Assert.IsType<int>(response);
            Assert.Equal(0, response);
        }
    }
}
