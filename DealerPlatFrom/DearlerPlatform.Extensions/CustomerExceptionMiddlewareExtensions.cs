using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace DearlerPlatform.Extensions
{
    public static class CustomerExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomerExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomerExceptionMiddleware>();
        }
    }
}