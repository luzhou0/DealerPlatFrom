using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DearlerPlatform.Common.MD5Module;
using DearlerPlatform.Service.CustomerApp.Dto;

namespace DearlerPlatform.Service.CustomerApp
{
    public partial class CustomerService: ICustomerService
    {
        public async Task<bool> CheckPassWord(CustomerLoginDto customerLoginDto)
        {
            var res = await _customerPwdReo.GetAsync(m => m.CustomerNo == customerLoginDto.CustomerNo && m.CustomerPwd1 == customerLoginDto.PassWord.ToMd5());
            if (res == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}