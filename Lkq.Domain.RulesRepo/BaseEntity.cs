using System;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Domain.RulesRepo
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public string CreatedUser { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedUser { get; set; }
    }
}
