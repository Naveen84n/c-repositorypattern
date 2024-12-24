using System.Diagnostics.CodeAnalysis;

namespace Lkq.Domain.RulesRepo
{
    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Attributes
    {
        /// <summary>
        /// ACES_Attributes_ID
        /// </summary>
        public int ACES_Attributes_ID { get; set; }
        /// <summary>
        /// TableName
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// KeyName
        /// </summary>
        public string KeyName { get; set; }
        /// <summary>
        /// ValueName
        /// </summary>
        public string ValueName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool AttributeStatus { get; set; }
    }
}
