using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API
{
    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType.FullName.Equals(typeof(Microsoft.AspNetCore.Http.IFormFile).FullName));

            if (fileParams.Any() && fileParams.Count() == 1)
            {
                operation.Parameters = new List<IParameter>
                {
                    new NonBodyParameter
                    {
                        Name = fileParams.First().Name,
                        Required = true,
                        Type = "file",
                        In = "formData"
                    }
                };
            }
        }
    }
}
