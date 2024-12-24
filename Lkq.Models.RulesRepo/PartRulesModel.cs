using System;
using System.Diagnostics.CodeAnalysis;

namespace Lkq.Models.RulesRepo
{

    /// <summary>
    /// Parts Rules Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PartRulesModel
    {
        /// <summary>
        /// Rule ID
        /// </summary>
        /// <example>19</example>
        public int RulesID { get; set; }

        /// <summary>
        /// Rule Description
        /// </summary>
        /// <example>Rule to filter Body Type</example>
        public string? RulesDescription { get; set; }

        /// <summary>
        /// Part Type
        /// </summary>
        /// <example>400</example>
        public string PartType { get; set; }

        /// <summary>
        /// Attribute ID
        /// </summary>
        /// <example>10</example>
        public int AttributesID { get; set; }

        /// <summary>
        /// AttributeName
        /// </summary>
        /// <example>BodyType</example>
        public string AttributeName { get; set; }

        /// <summary>
        /// Attribute Lookup
        /// </summary>
        /// <example>10</example>
        public string? AttributeLookup { get; set; }

        /// <summary>
        /// Data Source ID
        /// </summary>
        /// <example>2</example>
        public int DataSourceID { get; set; }

        /// <summary>
        /// Data Source Name
        /// </summary>
        /// <example>DataOne</example>
        public string DataSourceName { get; set; }

        /// <summary>
        /// Property path
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
        /// <example>13</example>
        public int? Ordinal { get; set; }

        /// <summary>
        /// Created date
        /// </summary>
        /// <example>2021-11-10T23:27:46.02</example>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Created User
        /// </summary>
        /// <example>Admin</example>
        public string CreatedUser { get; set; }

        /// <summary>
        /// Modified date
        /// </summary>
        /// <example>2021-11-10T23:27:46.02</example>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Modified User
        /// </summary>
        /// <example>Admin</example>
        public string? ModifiedUser { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>true</example>
        public bool IsActive { get; set; }

       
    }
}
