using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DearlerPlatform.Common.TokenModule;
using DearlerPlatform.Common.TokenModule.Models;
using DearlerPlatform.Service.CustomerApp;
using DearlerPlatform.Service.CustomerApp.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DearlerPlatform.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly IConfiguration _configuration;

        public LoginController(ICustomerService customerService, IConfiguration configuration)
        {
            this._customerService = customerService;
            this._configuration = configuration;
        }
        [HttpPost]
        public async Task<string> CheckLogin(CustomerLoginDto customerLoginDto)
        {
            if (string.IsNullOrEmpty(customerLoginDto.CustomerNo) || string.IsNullOrEmpty(customerLoginDto.PassWord))
            {
                HttpContext.Response.StatusCode = 400;
                return "NoLoginInfo";
            }
            var isSuccess = await _customerService.CheckPassWord(customerLoginDto);
            if (isSuccess)
            {
                //获取真实用户数据
                var customer = await _customerService.GetCustomerAsync(customerLoginDto.CustomerNo);
                var token = GetToken(customer.Id, customer.CustomerNo, customer.CustomerName);
                return token;
            }
            else
            {
                return "NoUserInfo";
            }
        }

        private string GetToken(int customerId, string customerNo, string customerName)
        {
            var token = _configuration.GetSection("Jwt").Get<JwtTokenModel>();
            token.Id = customerId;
            token.CustomerNo = customerNo;
            token.CustomerName = customerName;
            var jwtToken = TokenHelpers.CreateToken(token);
            return jwtToken;
        }
    }
}