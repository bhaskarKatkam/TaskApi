using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataParserApp.Models;

namespace DataParserApp.Repository
{
    public interface IDataparser
    {
        bool SaveData(List<Customer> req);
        List<Customer> GetDetails();
    }
}
