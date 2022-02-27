using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DearlerPlatform.Core.Repository;
using DearlerPlatform.Domain;

namespace DearlerPlatform.Service.CustomerApp
{
    public partial class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerReo;
        private readonly IRepository<CustomerPwd> _customerPwdReo;
        private readonly IRepository<CustomerInvoice> _customerInvoiceReo;

        public CustomerService(
         IRepository<Customer> customerReo,
         IRepository<CustomerPwd> customerPwdReo,
         IRepository<CustomerInvoice> customerInvoiceReo)
        {
            this._customerReo = customerReo;
            this._customerPwdReo = customerPwdReo;
            this._customerInvoiceReo = customerInvoiceReo;
        }

        public async Task<Customer> GetCustomerAsync(string customerNo)
        {
            return await _customerReo.GetAsync(m => m.CustomerNo == customerNo);
        }
    }
}