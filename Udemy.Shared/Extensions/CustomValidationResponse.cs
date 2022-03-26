using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Udemy.Shared.Dtos;

namespace Udemy.Shared.Extensions
{
    public static class CustomValidationResponse
    {
        public static void UseCustomValidationResponse(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(i=>i.Errors.Count>0).SelectMany(i=>i.Errors).Select(i=>i.ErrorMessage);

                    ErrorDto errorDto = new ErrorDto(errors.ToList(),true);

                    var response = Response<NoContentResult>.Fail(errorDto, 400);

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
