using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AttributesModel
    {
        /// <summary>
        /// Attributes ID
        /// </summary>
        /// <example>1</example>
        public int AttributeID { get; set; }
        /// <summary>
        /// Table Name
        /// </summary>
        /// <example>BedType</example>
        public string TableName { get; set; }
        /// <summary>
        /// Key Name
        /// </summary>
        /// <example>BedTypeID</example>
        public string KeyName { get; set; }
        /// <summary>
        /// Value Name
        /// </summary>
        /// <example>BedTypeName</example>
        public string ValueName { get; set; }

    }
}
