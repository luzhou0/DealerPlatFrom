using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using DearlerPlatform.Common.ErrorModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DearlerPlatform.Common.Filters
{
    public class ErrorHandleFilter : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(ExceptionContext context)
        {
            //如果异常没有被处理则进行处理
            if (context.ExceptionHandled == false)
            {
                //定义返回信息s
                var result = new ErrorResult
                {
                    ResultCode = 1000,
                    ResultMessage = context.Exception.Message
                };
                context.Result = new ContentResult
                {
                    //返回状态码设置200,表示成功
                    StatusCode = StatusCodes.Status200OK,
                    //设置返回格式
                    ContentType = "application/json;charest=utf-8",
                    Content = JsonSerializer.Serialize(result)
                };
            }
            //设置为true,表示异常都被处理了
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}