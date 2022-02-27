using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DearlerPlatform.Extensions
{
    /// <summary>
    /// 异常中间件返回信息
    /// </summary>
    public class ProblemDetails
    {
        public int StatusCode { get; set; }
        public string Title { get; set; }
        //public string Detail { get; set; }
        public Dictionary<string, object> Extensions { get; set; } = new();
    }
}