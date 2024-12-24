using FluentValidation;
using Lkq.Core.RulesRepo.Common;
using Lkq.Models.RulesRepo;

namespace Lkq.Api.RulesRepo.Validators
{
    /// <summary>
    /// Request Rule Model Validation
    /// </summary>
    public class RequestRuleModelValidator : AbstractValidator<RuleRequestModel>
    {

        /// <summary>
        /// Request Rule Validation constructor
        /// </summary>
        public RequestRuleModelValidator()
        {
            RuleFor(rule => rule.PartType).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_PARTTYPE);
            RuleFor(rule => rule.AttributeLookup).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_ATTRIBUTELOOKUP);
            RuleFor(rule => rule.AttributesID).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_ATTRIBUTEID);
            RuleFor(rule => rule.DataSourceID).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_DATASOURCEID);
            RuleFor(rule => rule.PropertyPath).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_PROPERTYPATH);
            RuleFor(rule => rule.PropertyValue).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_PROPERTYVALUE);
            RuleFor(rule => rule.RulesDescription).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_DESCRIPTION);
            RuleFor(rule => rule.User).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_USER);
            RuleFor(rule => rule.Ordinal).NotNull().NotEmpty().WithMessage(Constants.MESSAGE_INVALID_ORDINAL);
        }
    }
}
