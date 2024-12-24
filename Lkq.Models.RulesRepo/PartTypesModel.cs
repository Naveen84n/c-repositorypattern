using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{
    /// <summary>
    /// Part Codes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PartTypesModel
    {
        /// <summary>
        /// Part Code ID
        /// </summary>
        /// <example>1</example>
        public int PartTypeID { get; set; }

        /// <summary>
        /// Hollander Part No
        /// </summary>
        /// <example>300</example>
        public string PartType { get; set; }

        /// <summary>
        /// Part Code
        /// </summary>
        /// <example>ENG</example>
        public string PartTypeCode { get; set; }

        /// <summary>
        /// Part Code Description
        /// </summary>
        /// <example>Engine Assembly</example>
        public string PartTypeDescription { get; set; }

    }
}
