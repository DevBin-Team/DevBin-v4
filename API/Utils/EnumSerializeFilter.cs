using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Utils;

public class EnumSerializeFilter : ISchemaFilter
{ 

    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if(context.Type.IsEnum)
        {
            schema.Enum.Clear();
            string[] values = Enum.GetNames(context.Type);
            for (int i = 0; i < values.Length; i++)
            {
                schema.Enum.Add(new OpenApiString($"{i} ({values[i]})"));
            }
        }
    }
}