using DataParserApp.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataParserApp.Repository
{
    public class CustomerRepo : IDataparser
    {
        private readonly TestDBContext _dbContext;
        private readonly ILogger<CustomerRepo> _logger;
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(CustomerRepo));

        public CustomerRepo(TestDBContext dbContext, ILogger<CustomerRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public bool SaveData(List<Customer> req)
        {
            try
            {
                foreach (var obj in req)
                {
                    _dbContext.Customer.Add(obj);
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Customer> GetDetails()
        {
            try
            {
                List<Customer> obj = new List<Customer>();

                var data = _dbContext.Customer.OrderBy(e => e.CustName).ToList();

                obj = data.Count > 0 ? data : null;
                return obj;
            }
            catch
            {
                return null;
            }
        }
    }
}
