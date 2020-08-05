using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataParserApp.Models
{
    public class ResponseMessage
    {
        public string Message { set; get; }
        public object Status { set; get; }
        public object Data { set; get; }
    }
}
