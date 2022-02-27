using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DearlerPlatform.Common.EventBusHelper;
using DearlerPlatform.Common.Filters;
using DearlerPlatform.Common.TokenModule.Models;
using DearlerPlatform.Core;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Extensions;
using DearlerPlatform.Extensions.AutofacModule;
using DearlerPlatform.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
//属性注入配置
//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
//.ConfigureContainer<ContainerBuilder>(builder =>
//{
//    builder.RegisterModule(new AutofacModuleRegister());
//});
// Add services to the container.

builder.Services.AddControllers(options =>
{
    //自定义异常处理过滤器
    options.Filters.Add(typeof(ErrorHandleFilter));
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
builder.Services.AddSwaggerGen(c =>
{
    //添加安全定义
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "格式: Brarer {token}",
        Name = "Authorization",//默认的参数名
        In = ParameterLocation.Header,// 放于请求头中
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //添加安全要求
    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
      {
         new OpenApiSecurityScheme{
             Reference = new OpenApiReference{
             Type = ReferenceType.SecurityScheme,
             Id = "Bearer"
            }
         },Array.Empty<string>()
      }
    });
});
builder.Services.AddDbContext<DealerPlatformContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped(typeof(LocalEnevtBus<>));
builder.Services.RepositoryRegister();
builder.Services.ServiceRegister();
// 第一步：引入Nuget包 AutoMapper 和 AutoMapper.Extensions.Microsoft.DependencyInjection
// 第二步：创建映射类
// 第三步：将Automapper注册到系统中，并且添加实体映射类
builder.Services.AddAutoMapper(typeof(DealerPlatformProfile));
var token = builder.Configuration.GetSection("Jwt").Get<JwtTokenModel>();
#region Jwt验证
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(
    opt =>
    {
        //是否是HTTPS,默认为true
        opt.RequireHttpsMetadata = false;
        opt.SaveToken = true;
        opt.TokenValidationParameters = new()
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Security)),
            ValidIssuer = token.Issuer,
            ValidAudience = token.Audience
        };
        opt.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                //此处终止代码
                context.HandleResponse();
                var res = "{\"code\":401,\"err\"无权限\"}";
                context.Response.ContentType = "application/json";//格式为json
                context.Response.StatusCode = StatusCodes.Status203NonAuthoritative;
                context.Response.WriteAsync(res);
                return Task.FromResult(0);
            }
        };
    }
);
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("any");
//启用身份验证功能。
app.UseAuthentication();
app.UseHttpsRedirection();
//启用授权功能。
app.UseAuthorization();
//自定义异常处理中间件
app.UseCustomerExceptionMiddleware();
app.MapControllers();

app.Run();
