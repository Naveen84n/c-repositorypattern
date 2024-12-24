using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Domain.RulesRepo
{
    [ExcludeFromCodeCoverage]
    public class AttributeValues
    {
        /// <summary>
        /// Attributes Look Up Key
        /// </summary>
        /// <example>10</example>
        
        [Key]
        public int AttributeValue_ID
        {
            get; set;
        }

        /// <summary>
        /// Attributes Look Up Value
        /// </summary>
        /// <example>4 X 4</example>
        public string AttributeValue
        {
            get; set;
        }
    }
}
