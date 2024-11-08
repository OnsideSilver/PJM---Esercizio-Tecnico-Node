﻿using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.Annotations;
using Newtonsoft.Json;

namespace Node_ApiService_Test.Controllers.ControllerExtensions
{
    public class SwaggerIgnoreFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null)
            {
                return;
            }

            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var memberList = context.Type
                .GetFields(bindingFlags).Cast<MemberInfo>()
                .Concat(context.Type.GetProperties(bindingFlags));

            var excludedList = memberList
                .Where(m => m.GetCustomAttribute<SwaggerSchemaAttribute>() != null)
                .Select(m => m.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? m.Name.ToCamelCase());

            foreach (var excludedName in excludedList)
            {
                if (schema.Properties.ContainsKey(excludedName))
                    schema.Properties.Remove(excludedName);
            }
        }
    }

    public static class StringExtension
    {
        public static string ToCamelCase(this string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str.ToLowerInvariant();
        }
    }
}
