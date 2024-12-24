using Lkq.Models.RulesRepo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lkq.Core.RulesRepo.Common
{
    public static class StructureHelper
    {
        public static bool Simple(Type type)
        {
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }

        public static IList<DataSourceStructure> ExtractStructure(Type type)
        {
            var response = new List<DataSourceStructure>();
            if (Simple(type))
                return response;
            var properties = type.GetProperties();
            var fields = type.GetFields();
            foreach (var propertyInfo in properties)
            {
                var subtype = propertyInfo.PropertyType;
                if (subtype.IsArray)
                    subtype = subtype.GetElementType();
                else if (subtype != typeof(String) && subtype.GetInterfaces().Contains(typeof(IEnumerable)))
                    subtype = subtype.GetGenericArguments()[0];
                response.Add(new DataSourceStructure(propertyInfo.Name, ExtractStructure(subtype)));
            }

            foreach (var fieldInfo in fields)
            {
                var subtype = fieldInfo.FieldType;
                if (subtype.IsArray)
                    subtype = subtype.GetElementType();
                else if (subtype != typeof(String) && subtype.GetInterfaces().Contains(typeof(IEnumerable)))
                    subtype = subtype.GetGenericArguments()[0];
                response.Add(new DataSourceStructure(fieldInfo.Name, ExtractStructure(subtype)));
            }

            return response.OrderBy(r => r.Name).ToList();
        }
    }
}
