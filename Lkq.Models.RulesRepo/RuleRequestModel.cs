using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{

    /// <summary>
    /// Rule request
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RuleRequestModel
    {
        /// <summary>
        /// Rules Description
        /// </summary>
        /// <example>Rule to filter Body Type</example>
        public string RulesDescription { get; set; }

        /// <summary>
        /// Part Type
        /// </summary>
        /// <example>300</example>
        public string PartType { get; set; }

        /// <summary>
        /// Attribute ID
        /// </summary>
        /// <example>10</example>
        public int AttributesID { get; set; }

        /// <summary>
        /// Attribute Lookup
        /// </summary>
        /// <example>22</example>
        public string AttributeLookup { get; set; }

        /// <summary>
        /// Data Source ID
        /// </summary>
        /// <example>2</example>
        public int DataSourceID { get; set; }

        /// <summary>
        /// Property Path
        /// </summary>
        /// <example>query_responses.common_data.basic_data.body_type</example>
        public string PropertyPath { get; set; }

        /// <summary>
        /// Property Value
        /// </summary>
        /// <example>Convertible</example>
        public string PropertyValue { get; set; }

        /// <summary>
        /// Ordinal
        /// </summary>
        /// <example><2/example>
        public int Ordinal { get; set; }

        /// <summary>
        /// User Name
        /// </summary>
        /// <example>Admin</example>
        public string User { get; set; }

    }
}
