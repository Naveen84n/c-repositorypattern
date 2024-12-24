using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{
    /// <summary>
    /// Attribute Values
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AttributeValuesModel
    {
        /// <summary>
        /// Attribute Values Key
        /// </summary>
        /// <example>10</example>
        public int AttributeValueID { get; set; }

        /// <summary>
        /// Attribute Values Value
        /// </summary>
        /// <example>4 X 4</example>
        public string AttributeValue { get; set; }
    }
}
