using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{
    /// <summary>
    /// Data Source Structure
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class DataSourceStructure
    {

        /// <summary>
        /// Property name
        /// </summary>
        /// <example>decoder_messages</example>
        public string Name { get; set; }

        /// <summary>
        /// Property Child objects
        /// </summary>
        public IList<DataSourceStructure> Children { get; set; } = new List<DataSourceStructure>();


        /// <summary>
        /// DataSource Structure Constructor 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="children"></param>
        public DataSourceStructure(string name, IList<DataSourceStructure> children)
        {
            Name = name;
            Children = children;
        }
    }
}
