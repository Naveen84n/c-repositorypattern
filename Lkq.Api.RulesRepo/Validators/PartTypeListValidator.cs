using FluentValidation;
using Lkq.Core.RulesRepo.Common;
using System.Collections.Generic;
using System.Linq;

namespace Lkq.Api.RulesRepo.Validators
{
    /// <summary>
    /// Part Type List Validation
    /// </summary>
    public class PartTypeListValidator : AbstractValidator<List<string>>
    {
        /// <summary>
        /// Part Type List constructor
        /// </summary>
        public PartTypeListValidator()
        {
            RuleFor(x => x).NotNull().WithMessage(Constants.MESSAGE_INVALID_PARTTYPELIST);
            RuleForEach(x => x).NotEmpty().WithMessage(Constants.MESSAGE_INVALID_PARTTYPELIST);
            RuleFor(x => x).Must(c => c.Count >= 1).WithMessage(Constants.MESSAGE_INVALID_PARTTYPELIST);
        }
    }
}
