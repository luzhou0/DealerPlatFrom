using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DearlerPlatform.Extensions.AutofacModule
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //程序集注入业务服务
            var IAppServices = Assembly.Load("DearlerPlatform.Service");
            var AppServices = Assembly.Load("DearlerPlatform.Service");
            builder.RegisterAssemblyTypes(IAppServices, AppServices).Where(t=>t.Name.EndsWith("Service")).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes();
        }
    }
}
