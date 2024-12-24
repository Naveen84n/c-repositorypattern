using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Lkq.Api.RulesRepo.Model;
using Lkq.Models.RulesRepo;
using Lkq.Core.RulesRepo.Interfaces;
using Lkq.Api.RulesRepo.Extension;
using Lkq.Core.RulesRepo.Common;
using Lkq.Api.RulesRepo.Validators;

namespace Lkq.Api.RulesRepo.Controllers
{
    /// <summary>
    /// Rules Controller
    /// </summary>
    [ApiController]
    [ApiVersion("1")]
    [Authorize]
    [Route("/v1/[controller]")]
    public class RulesController : ControllerBase
    {
        private IRulesRepoApiResponse _apiResponse = null;
        private readonly ILogger<RulesController> _logger;
        private readonly IRules _rules;

        /// <summary>
        /// Rules Controller Constructor
        /// </summary>
        /// <param name="apiResponse"></param>
        /// <param name="logger"></param>
        /// <param name="rules"></param>
        public RulesController(IRulesRepoApiResponse apiResponse, ILogger<RulesController> logger, IRules rules)
        {
            _apiResponse = apiResponse;
            _logger = logger;
            _rules = rules;
        }

        /// <summary>
        /// Returns list of Part Types
        /// </summary>
        /// <remarks>Returns list of Part Types.</remarks>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/v1/Rules/PartTypes")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PartTypesModel>), description: "Successful Operation")]
        public async Task<IActionResult> GetPartTypes()
        {
            _logger.LogInformation("Fetching Part Types");
            var partTypes = await _rules.GetPartTypes();
            return Ok(_apiResponse.CreateAPISuccessResponse(partTypes));
        }

        /// <summary>
        /// Returns list of Attributes
        /// </summary>
        /// <remarks>Returns list of Attributes.</remarks>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/v1/Rules/Attributes")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(AttributesModel), description: "successful operation")]
        public async Task<IActionResult> GetAttributes()
        {
            _logger.LogInformation("Fetching rules Attributes tables");
            var attributes = await _rules.GetAttributes();
            return Ok(_apiResponse.CreateAPISuccessResponse(attributes));
        }

        /// <summary>
        /// Returns list of Attribute Values for the given Attribute
        /// </summary>
        /// <param name="attributeName"></param>
        /// <remarks>
        /// Returns list of Attribute Values for the given Attribute
        /// 
        /// Sample request:
        /// 
        ///     attributeName : "BedType"
        /// </remarks>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="400">Bad Request</response>
        [HttpGet]
        [Route("/v1/Rules/AttributeValues")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AttributeValuesModel>), description: "Successful Operation")]
        public async Task<IActionResult> GetAttributesValue([FromQuery][Required] string attributeName)
        {
            attributeName.CheckNullOrEmpty(Constants.MESSAGE_INVALID_ATTRIBUTENAME);
            _logger.LogInformation("Fetching Attribute Values");
            var attributeValues = await _rules.GetAttributeValues(attributeName);

            return attributeValues.Any()
              ? Ok(_apiResponse.CreateAPISuccessResponse(attributeValues))
              : Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_ATTRIBUTEINFO_NOTAVAILABLE));

        }

        /// <summary>
        /// Returns list of Data sources with the response structure
        /// </summary>
        /// <remarks>Returns list of Data sources with the response structure.</remarks>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/v1/Rules/DataSources")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<DataSourceModel>), description: "successful operation")]
        public async Task<IActionResult> GetDataSources()
        {
            _logger.LogInformation("Fetching list of data sources with structure");
            var dataSources = await _rules.GetDataSources();
            return Ok(_apiResponse.CreateAPISuccessResponse(dataSources));
        }

        /// <summary>
        /// Returns rule by Rule ID
        /// </summary>
        /// <remarks> 
        /// Returns rule by Rule ID
        /// 
        /// Sample request:
        /// 
        ///     id : 1
        /// </remarks>
        /// <param name="id"></param>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/v1/Rules/{id}")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(PartRulesModel), description: "successful operation")]
        public async Task<IActionResult> GetRule([FromRoute] int id)
        {
            id.CheckLessThanOrEqual(0, Constants.MESSAGE_INVALID_RULEID);

            _logger.LogInformation("Fetching rule by rule ID");
            var rule = await _rules.GetRule(id);

            return rule == null
                ? Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULE_NOTAVAILABLE))
                : Ok(_apiResponse.CreateAPISuccessResponse(rule));
        }

        /// <summary>
        /// Returns list of rules by Part Type and Attribute ID 
        /// </summary>
        /// <param name="attributeID"></param>
        /// <param name="partType"></param>
        /// <remarks>
        /// Returns list of rules by Part Type and Attribute ID
        /// 
        ///     Attribute ID and Part Types are optional parameter, if both the parameters are null then it will return all the rules else
        ///     filters applied based on the input parameter 
        /// Sample request:
        /// 
        ///     attributeID : 1
        ///     partType : 400
        /// </remarks>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/v1/Rules")]
        [Produces("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<PartRulesModel>), description: "successful operation")]
        public async Task<IActionResult> GetRules([FromQuery] int attributeID, [FromQuery] int partType)
        {
            attributeID.CheckLessThanOrEqual(-1, Constants.MESSAGE_INVALID_ATTRIBUTEID);
            partType.CheckLessThanOrEqual(-1, Constants.MESSAGE_INVALID_PARTTYPE);

            _logger.LogInformation("Fetching rules by Part Type and Attribute ID");
            var rules = await _rules.GetRules(attributeID, partType);

            return rules.Any()
                ? Ok(_apiResponse.CreateAPISuccessResponse(rules))
                : Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULES_NOTAVAILABLE));
        }

        /// <summary>
        /// Creates a new Rule
        /// </summary>
        /// <remarks>
        /// Creates a new Rule and returns the newly created data
        /// 
        /// Sample request:
        /// 
        ///     {
        ///      "rulesDescription": "BedType filter",
        ///      "partType": "101",
        ///      "attributesID": 9,
        ///      "attributeName": "BedType",
        ///      "attributeLookup": "11",
        ///      "dataSourceID": 1,
        ///      "propertyPath": "RequestedData.CompNineData.BuildDate",
        ///      "propertyValue": "1",
        ///      "ordinal": 1,
        ///      "user": "Admin"
        ///     }
        /// </remarks>
        /// <param name="body"></param>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/v1/Rules")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(statusCode: 201, type: typeof(PartRulesModel), description: "Successfully Created")]
        public async Task<IActionResult> SaveRule([FromBody] RuleRequestModel body)
        {
            body.CheckObjNull(Constants.STATUSMESSAGE_INVALID_REQUEST);

            RequestRuleModelValidator _requestValidator = new();
            var validationresult = _requestValidator.Validate(body);
            if (!validationresult.IsValid)
                return BadRequest(_apiResponse.CreateAPIFailureResponse(validationresult.Errors[0].ToString()));

            var response = await _rules.SaveRule(body);

            return response.RulesID != 0
              ? Created("/v1/Rules/" + response.RulesID, _apiResponse.CreatedResponse(response))
              : Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS));
        }

        /// <summary>
        /// Returns no. of rules by list of partTypes
        /// </summary>
        /// <remarks> 
        /// Returns no. of rules by list of partTypes, "All" is to return total count of rules
        /// 
        /// Sample request:
        /// 
        ///     partTypes : ["300","400"]
        ///     
        /// </remarks>
        /// <param name="partType"></param>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        [Route("/v1/Rules/Count")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(int), description: "successful operation")]
        public async Task<IActionResult> GetCount([FromBody] List<string> partType)
        {
            partType.CheckObjNull(Constants.STATUSMESSAGE_INVALID_REQUEST);

            PartTypeListValidator _requestValidator = new();
            var validationresult = _requestValidator.Validate(partType);
            if (!validationresult.IsValid)
                return BadRequest(_apiResponse.CreateAPIFailureResponse(validationresult.Errors[0].ToString()));

            _logger.LogInformation("Fetching rule count by partType");
            var rule = await _rules.GetCount(partType);

            return rule == 0
                ? Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULES_NOTAVAILABLE))
                : Ok(_apiResponse.CreateAPISuccessResponse(rule));
        }

        /// <summary>
        /// Updates an existing rule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="body"></param>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Updates an existing Rule
        /// 
        /// Sample request:
        ///     
        ///     id : 250
        /// 
        ///     {
        ///      "rulesDescription": "BedType filter",
        ///      "partType": "101",
        ///      "attributesID": 9,
        ///      "attributeName": "BedType",
        ///      "attributeLookup": "11",
        ///      "dataSourceID": 1,
        ///      "propertyPath": "RequestedData.CompNineData.BuildDate",
        ///      "propertyValue": "1",
        ///      "ordinal": 1,
        ///      "user": "Admin"
        ///     }
        /// </remarks>
        [HttpPut]
        [Route("/v1/Rules/{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(PartRulesModel), description: "Successfully Updated")]
        public async Task<IActionResult> UpdateRule([FromRoute] int id, RuleRequestModel body)
        {
            id.CheckLessThanOrEqual(0, Constants.MESSAGE_INVALID_RULEID);
            body.CheckObjNull(Constants.STATUSMESSAGE_INVALID_REQUEST);

            RequestRuleModelValidator _requestValidator = new();
            var validationresult = _requestValidator.Validate(body);
            if (!validationresult.IsValid)
                return BadRequest(_apiResponse.CreateAPIFailureResponse(validationresult.Errors[0].ToString()));

            var response = await _rules.UpdateRule(id, body);

            return response == null ? Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULE_NOTAVAILABLE))
                : response.IsActive == false ? Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULE_ALREADYEXISTS))
                : Ok(_apiResponse.CreateAPISuccessResponse(response));
        }

        /// <summary>
        /// Deletes an existing rule
        /// </summary>
        /// <param name="id"></param>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized Access</response>
        /// <response code="404">Not Found</response>
        /// <response code="500">Internal Server Error</response>
        /// <remarks>
        /// Deletes an existing RUle 
        /// 
        /// Sample Request:
        ///     
        ///     id : 250
        /// </remarks>
        [HttpDelete]
        [Route("/v1/Rules/{id}")]
        [Produces("application/json")]
        [Consumes("application/json")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Successfully Deleted")]
        public async Task<IActionResult> DeleteRule([FromRoute] int id)
        {
            id.CheckLessThanOrEqual(0, Constants.MESSAGE_INVALID_RULEID);
            var response = await _rules.DeleteRule(id);
            
            return response == 0 ? NotFound(_apiResponse.CreateAPIFailureResponse(Constants.MESSAGE_RULE_NOTFOUND, Constants.NOTFOUND))
                : Ok(_apiResponse.CreateAPISuccessResponse(Constants.MESSAGE_RULE_DELETED));
        }

    }
}
