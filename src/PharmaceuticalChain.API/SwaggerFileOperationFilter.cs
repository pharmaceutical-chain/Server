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
            if (operation.OperationId == "Post")
            {
                operation.Parameters = new List<IParameter>
                {
                    new NonBodyParameter
                    {
                        Name = "myFile",
                        Required = true,
                        Type = "file",
                        In = "formData"
                    }
                };
            }
        }
    }
}
