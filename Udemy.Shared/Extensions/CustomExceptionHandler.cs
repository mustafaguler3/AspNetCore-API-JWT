using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Udemy.Shared.Dtos;

namespace Udemy.Shared.Extensions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                config.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (errorFeature != null)
                    {
                        var ex = errorFeature.Error;
                        ErrorDto errorDto = new ErrorDto();
                    }

                    var response = Response<NoDataDto>.Fail(errorDto, 500);

                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }); //run sonlandırıcı bir middleware demek 
            });
        }
    }
}
