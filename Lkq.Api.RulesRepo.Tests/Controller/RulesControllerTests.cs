using System.Threading.Tasks;
using System.Collections.Generic;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lkq.Api.RulesRepo.Controllers;
using Lkq.Api.RulesRepo.Model;
using Lkq.Core.RulesRepo.Interfaces;
using Lkq.Core.RulesRepo.Common;
using Lkq.Models.RulesRepo;
using System;

namespace Lkq.Api.RulesRepo.Tests.Controller
{
    public class RulesControllerTests
    {
        private RulesRepoApiResponse GetTestSucessResponse(dynamic result)
        {
            var type = result.GetType().Equals(typeof(string));

            return new RulesRepoApiResponse
            {
                StatusCode = Constants.OK,
                Message = type ? result : Constants.SUCCESS,
                Result = !type ? result : null
            };
        }

        private RulesRepoApiResponse GetTestCreatedResponse(dynamic result)
        {
            return new RulesRepoApiResponse
            {
                StatusCode = Constants.CREATED,
                Message = Constants.STATUSMESSAGE_CREATED,
                Result = result
            };
        }

        private static RulesRepoApiResponse GetTestFailureResponse(string error = null)
        {
            var statuscode = string.IsNullOrEmpty(error) ?
                Constants.SERVERERROR : Constants.BADREQUEST;
            var message = string.IsNullOrEmpty(error) ?
             Constants.STATUSMESSAGE_SERVER_ERROR : error;

            return new RulesRepoApiResponse
            {
                StatusCode = statuscode,
                Message = message
            };
        }

        private readonly RuleRequestModel ruleRequest = new()
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

        private readonly RuleRequestModel invalidPartTypeRuleRequest = new()
        {
            PartType = "",
            AttributesID = 21,
            AttributeLookup = "751",
            DataSourceID = 1,
            PropertyPath = "RequestedData.CompNineData.OptionList.Value",
            PropertyValue = "4R70W",
            Ordinal = 5,
            User = "Admin",
            RulesDescription = "RuleDesc"
        };

        [Fact]
        public async Task GetAttributes_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetAttributes();

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetPartTypes_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetPartTypes();

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetAttributesValue_ValidAttribute_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            var attributeName = "DriveType";
            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                 .Returns(GetTestSucessResponse("Success"));

            //Act
            var response = await controller.GetAttributesValue(attributeName);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetAttributesValue_InValidAttribute_ReturnsNotAvailable()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var attributeName = "DriveType";

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(It.IsAny<string>()))
                 .Returns(GetTestSucessResponse(Constants.MESSAGE_ATTRIBUTEINFO_NOTAVAILABLE));
            _mockService.Setup(x => x.GetAttributeValues(It.IsAny<string>()))
                .ReturnsAsync(new List<AttributeValuesModel>());
            //Act
            var response = await controller.GetAttributesValue(attributeName);

            //Assert  
            var model = Assert.IsType<OkObjectResult>(response).Value as RulesRepoApiResponse;
            Assert.Equal(200, model.StatusCode);
            Assert.Equal(Constants.MESSAGE_ATTRIBUTEINFO_NOTAVAILABLE, model.Message as string);
        }

        [Fact]
        public async Task GetRule_ValidPartRulesID_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            int id = 230;
            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetRule(id);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetRule_InValidPartRulesID_ReturnsNotAvailable()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            int id = 2;
            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(It.IsAny<string>()))
                   .Returns(GetTestSucessResponse(Constants.MESSAGE_RULE_NOTAVAILABLE));

            _mockService.Setup(x => x.GetRule(It.IsAny<int>()))
                        .ReturnsAsync(value: null);

            //Act
            var response = await controller.GetRule(id);

            //Assert  
            var model = Assert.IsType<OkObjectResult>(response).Value as RulesRepoApiResponse;
            Assert.Equal(200, model.StatusCode);
            Assert.Equal(Constants.MESSAGE_RULE_NOTAVAILABLE, model.Message as string);
        }

        [Fact]
        public async Task GetRules_ValidRequest_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            int attributeID = 2;
            int partType = 300;

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetRules(attributeID, partType);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetRules_ValidRequest_ReturnsNotAvailable()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            int attributeID = 2;
            int partType = 300;

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(It.IsAny<string>()))
                  .Returns(GetTestSucessResponse(Constants.MESSAGE_RULES_NOTAVAILABLE));

            _mockService.Setup(x => x.GetRules(It.IsAny<int>(), It.IsAny<int>()))
                        .ReturnsAsync(new List<PartRulesModel>());
            //Act
            var response = await controller.GetRules(attributeID, partType);

            //Assert  
            var model = Assert.IsType<OkObjectResult>(response).Value as RulesRepoApiResponse;
            Assert.Equal(200, model.StatusCode);
            Assert.Equal(Constants.MESSAGE_RULES_NOTAVAILABLE, model.Message as string);
        }

        [Fact]
        public async Task GetDataSources_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetDataSources();

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task SaveRule_ValidRequest_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreatedResponse("Success"))
                .Returns(GetTestCreatedResponse("Success"));

            _mockService.Setup(x => x.SaveRule(It.IsAny<RuleRequestModel>()))
                .ReturnsAsync(new PartRulesModel { RulesID = 1 });
            //Act
            var response = await controller.SaveRule(ruleRequest);

            //Assert  
            var viewResult = Assert.IsType<CreatedResult>(response);
            Assert.Equal(201, viewResult.StatusCode);
        }


        [Fact]
        public async Task SaveRule_ValidRequest_ReturnsAlreadyExists()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS))
                .Returns(GetTestSucessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS));

            _mockService.Setup(x => x.SaveRule(It.IsAny<RuleRequestModel>()))
                .ReturnsAsync(new PartRulesModel { RulesID = 0 });
            //Act
            var response = await controller.SaveRule(ruleRequest);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<OkObjectResult>(response).Value;
            Assert.Equal(200, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_RULE_ALREADYEXISTS, viewResult.Message);
        }

        [Fact]
        public async Task SaveRule_InvalidRequest_ReturnsBadRequestResult()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            _mockApiResponse.Setup(x => x.CreateAPIFailureResponse(Constants.MESSAGE_INVALID_PARTTYPE,0))
                 .Returns(GetTestFailureResponse(Constants.MESSAGE_INVALID_PARTTYPE));

            //Act
            var response = await controller.SaveRule(invalidPartTypeRuleRequest);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<BadRequestObjectResult>(response).Value;
            Assert.Equal(400, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_INVALID_PARTTYPE, viewResult.Message);
        }

        [Fact]
        public async Task UpdateRule_ValidRequest_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreatedResponse("Success"))
                .Returns(GetTestCreatedResponse("Success"));

            _mockService.Setup(x => x.UpdateRule(It.IsAny<int>(), It.IsAny<RuleRequestModel>()))
                .ReturnsAsync(new PartRulesModel { IsActive = true });
            //Act
            var response = await controller.UpdateRule(id, ruleRequest);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task UpdateRule_ValidRequest_ReturnsAlreadyExists()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS))
                .Returns(GetTestSucessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS));

            _mockService.Setup(x => x.UpdateRule(It.IsAny<int>(), It.IsAny<RuleRequestModel>()))
                .ReturnsAsync(new PartRulesModel { IsActive = false });
            //Act
            var response = await controller.UpdateRule(id, ruleRequest);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<OkObjectResult>(response).Value;
            Assert.Equal(200, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_RULE_ALREADYEXISTS, viewResult.Message);
        }

        [Fact]
        public async Task UpdateRule_ValidRequest_ReturnsRuleNotAvailable()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(Constants.MESSAGE_RULE_NOTAVAILABLE))
                .Returns(GetTestSucessResponse(Constants.MESSAGE_RULE_NOTAVAILABLE));

            _mockService.Setup(x => x.UpdateRule(It.IsAny<int>(), It.IsAny<RuleRequestModel>()));
            //Act
            var response = await controller.UpdateRule(id, ruleRequest);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<OkObjectResult>(response).Value;
            Assert.Equal(200, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_RULE_NOTAVAILABLE, viewResult.Message);
        }

        [Fact]
        public async Task UpdateRule_InvalidRequest_ReturnsBadRequestResult()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreateAPIFailureResponse(Constants.MESSAGE_INVALID_PARTTYPE,0))
                 .Returns(GetTestFailureResponse(Constants.MESSAGE_INVALID_PARTTYPE));

            //Act
            var response = await controller.UpdateRule(id, invalidPartTypeRuleRequest);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<BadRequestObjectResult>(response).Value;
            Assert.Equal(400, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_INVALID_PARTTYPE, viewResult.Message);
        }

        [Fact]
        public async Task DeleteRule_ValidRequest_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();

            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreatedResponse("Success"))
                .Returns(GetTestCreatedResponse("Success"));

            _mockService.Setup(x => x.DeleteRule(It.IsAny<int>()))
                .ReturnsAsync(1);

            //Act
            var response = await controller.DeleteRule(id);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task DeleteRule_InvalidRequest_ReturnsRuleNotAvailable()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);

            var id = 297;

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(Constants.MESSAGE_RULE_NOTFOUND))
                 .Returns(GetTestSucessResponse(Constants.MESSAGE_RULE_NOTFOUND));

            _mockService.Setup(x => x.DeleteRule(It.IsAny<int>()))
                .ReturnsAsync(value: 0);

            //Act
            var response = await controller.DeleteRule(id);

            //Assert  
            var viewResult = Assert.IsType<NotFoundObjectResult>(response);
            Assert.Equal(404, viewResult.StatusCode);
        }

          [Fact]
        public async Task GetRulesCount_ValidPartTypesList_ReturnsOkResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            List<string> partType = new List<string> { "300" };

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse("Success"))
                   .Returns(GetTestSucessResponse("Success"));
            //Act
            var response = await controller.GetCount(partType);

            //Assert  
            var viewResult = Assert.IsType<OkObjectResult>(response);
            Assert.Equal(200, viewResult.StatusCode);
        }

        [Fact]
        public async Task GetRulesCount_InvalidPartTypes_ReturnsCountZeroResults()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            List<string> partType = new() { "200" };

            _mockApiResponse.Setup(x => x.CreateAPISuccessResponse(Constants.MESSAGE_RULES_NOTAVAILABLE))
                   .Returns(GetTestSucessResponse(Constants.MESSAGE_RULES_NOTAVAILABLE));
            //Act
            var response = await controller.GetCount(partType);

            //Assert  
            var viewResult = (RulesRepoApiResponse) Assert.IsType<OkObjectResult>(response).Value;
            Assert.Equal(Constants.MESSAGE_RULES_NOTAVAILABLE, viewResult.Message);
        }

        [Fact]
        public async Task GetRulesCount_InvalidPartTypes_ReturnsBadRequest()
        {
            //Arrange  
            var _mockService = new Mock<IRules>();
            var _mocklogger = new Mock<ILogger<RulesController>>();
            var _mockApiResponse = new Mock<IRulesRepoApiResponse>();
            var controller = new RulesController(_mockApiResponse.Object, _mocklogger.Object,
                                _mockService.Object);
            List<string> partType = new() { "" };

            _mockApiResponse.Setup(x => x.CreateAPIFailureResponse(It.IsAny<string>(),0))
                   .Returns(GetTestFailureResponse(Constants.MESSAGE_INVALID_PARTTYPELIST));
            //Act
            var response = await controller.GetCount(partType);

            //Assert  
            var viewResult = (RulesRepoApiResponse)Assert.IsType<BadRequestObjectResult>(response).Value;
            Assert.Equal(Constants.BADREQUEST, viewResult.StatusCode);
            Assert.Equal(Constants.MESSAGE_INVALID_PARTTYPELIST, viewResult.Message);
        }

    }
}
