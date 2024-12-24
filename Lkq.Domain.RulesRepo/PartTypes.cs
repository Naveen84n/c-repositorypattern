using System.Diagnostics.CodeAnalysis;

namespace Lkq.Domain.RulesRepo
{
    /// <summary>
    /// Part Codes
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PartTypes
    {
        /// <summary>
        /// Part Code ID
        /// </summary>
        public int Part_Codes_ID { get; set; }

        /// <summary>
        /// Hollander Part No
        /// </summary>
        public string HollanderPartNo { get; set; }

        /// <summary>
        /// Part Code
        /// </summary>
        public string PartCode { get; set; }

        /// <summary>
        /// Part Code Description
        /// </summary>
        public string PartCodeDescription { get; set; }

        /// <summary>
        /// Part Group
        /// </summary>
        public string PartGroup { get; set; }

        /// <summary>
        /// Mechanical
        /// </summary>
        public string Mechanical { get; set; }

        /// <summary>
        /// SheetMetal
        /// </summary>
        public string SheetMetal { get; set; }

        /// <summary>
        /// Interior
        /// </summary>
        public string Interior { get; set; }

        /// <summary>
        /// Sided
        /// </summary>
        public string Sided { get; set; }

        /// <summary>
        /// HasWarranty
        /// </summary>
        public string HasWarranty { get; set; }

        /// <summary>
        /// Vertex Tax Group
        /// </summary>
        public string VertexTaxGroup { get; set; }
    }
}
