using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Domain.RulesRepo
{
    [ExcludeFromCodeCoverage]
    public class PartRules : BaseEntity
    {
        public int Part_Rules_ID { get; set; }

        public string PartType { get; set; }

        public int ACES_Attributes_ID { get; set; }

        public string? AttributeLookup { get; set; }

        public int Vindecoder_Source_ID { get; set; }

        public string PropertyPath { get; set; }

        public string PropertyValue { get; set; }

        public int? Ordinal { get; set; }

        public bool IsActive { get; set; }

        public string? RulesDescription { get; set; }

        [ForeignKey("ACES_Attributes_ID")]
        public virtual Attributes Attributes { get; set; }

        [ForeignKey("Vindecoder_Source_ID")]
        public virtual DataSource DataSource { get; set; }
    }
}
