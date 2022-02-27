using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DearlerPlatform.Extensions
{
    /// <summary>
    /// 自定义异常中间件
    /// </summary>
    public class CustomerExceptionMiddleware
    {

        private readonly RequestDelegate _next;
        /// <summary>
        /// 注入下一个中间件
        /// </summary>
        /// <param name="next"></param>
        public CustomerExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                //等待下一个中间件执行完成
                await _next(context);
            }
            catch (Exception ex)
            {

                context.Response.ContentType = "application/problem+json";
                var problem = new ProblemDetails
                {
                    StatusCode = 500,
                    Title = "Exception" + ex.Message
                };
                problem.Extensions.Add("message",ex.Message);
                if (!string.IsNullOrWhiteSpace(ex.StackTrace))
                {
                    //自定义正则匹配 at [方法名] in [文件路径]:line[行号]
                    var matches = Regex.Matches(ex.StackTrace, "at[ ](.+?)[ ]in[ ](.+?)[:]line[ ]([0-9]+)");
                    //使用linq的扩展方法,筛选出需要的数据
                    //过滤掉:
                    //包含当前中间件名字的错误信息
                    var filterMatches = matches.Where(t => !t.Groups[0].Value.ToString().Contains("DearlerPlatform.Extensions.CustomerExceptionMiddleware"));
                    //投影到新的对象列表,方便前端人员阅读
                    var extendExObject = filterMatches.Select(x => new
                    {
                        method = x.Groups[1].Value.ToString(),
                        file = x.Groups[2].Value.ToString(),
                        line = x.Groups[3].Value.ToString()
                    });
                    //往problem对象添加一个拓展属性detail
                    problem.Extensions.Add("detail", extendExObject);
                    var stream = context.Response.Body;
                    await JsonSerializer.SerializeAsync(stream, problem);
                }
            }
        }
    }
}