using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Domain;
using DearlerPlatform.Service.CustomerApp.Dto;

namespace DearlerPlatform.Service.CustomerApp
{
    public interface ICustomerService:IocTag
    {
        //登录检查
        public Task<bool> CheckPassWord(CustomerLoginDto customerLoginDto);
        //获取用户信息
        public Task<Customer> GetCustomerAsync(string customerNo);
    }
}