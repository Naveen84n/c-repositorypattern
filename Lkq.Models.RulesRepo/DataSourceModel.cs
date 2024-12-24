using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{

    /// <summary>
    /// Data Source Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataSourceModel
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public DataSourceModel(int id, string value)
        {
            DataSourceID = id;
            DataSource = value;
        }

        /// <summary>
        /// Data Source ID
        /// </summary>
        /// <example>1</example>
        public int DataSourceID { get; set; }

        /// <summary>
        /// Data Source
        /// </summary>
        /// <example>CompNine</example>
        public string DataSource { get; set; }

        /// <summary>
        /// Data Source Structure
        /// </summary>
        public IList<DataSourceStructure> Structure { get; set; }
    }
}
