using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataParserApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataParserApp.Repository;
using System.Text;

namespace DataParserApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataParserController : ControllerBase
    {
        private readonly IDataparser _repo;

        private readonly ILogger<DataParserController> _logger;
        private static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(DataParserController));

        public DataParserController(IDataparser repo, ILogger<DataParserController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        [HttpPost]
        [Route("ParseInputFileData")]
        public ResponseMessage ReadData(IFormFile inputFile, decimal minSalesAmt)
        {
            ResponseMessage resp = new ResponseMessage();
            string msg = "";
            try
            {
                if (inputFile != null && inputFile.Length > 0)
                {
                    List<Customer> obj = new List<Customer>();
                    bool ret = false;

                    using (StreamReader file = new StreamReader(inputFile.OpenReadStream()))
                    {
                        int counter = 0;

                        string ln;

                        while ((ln = file.ReadLine()) != null)
                        {
                            if (ln.ToString().StartsWith("#") == false && ln.ToString().StartsWith(";") == false && ln.ToString().StartsWith(":") == false)
                            {
                                var objArray = ln.Split(",");
                                if (objArray.Length == 5)
                                {
                                    var objDate = objArray[4].Split("-");
                                    try
                                    {
                                        int _year = Convert.ToInt32(RemoveBraces(objDate[0]));
                                        int _month = Convert.ToInt32(RemoveBraces(objDate[1]));
                                        int _day = Convert.ToInt32(RemoveBraces(objDate[2]));
                                        DateTime dt = new DateTime(_year, _month, _day);
                                        DateTime now = DateTime.Now;

                                        int flag = 0;

                                        if (dt > now)
                                        {
                                            flag = 1;
                                            msg += "Error: Timestamp of the customer: " + objArray[0] + " is not earlier than the current date \n";
                                        }
                                        else if (Convert.ToDecimal(objArray[3]) < minSalesAmt)
                                        {
                                            flag = 1;
                                            msg += "Error: Timestamp of the customer: " + objArray[0] + " is not earlier than the current date \n";
                                        }

                                        if (flag == 0)
                                        {
                                            obj.Add(new Customer
                                            {
                                                CustId = Convert.ToInt32(objArray[0]),
                                                CustType = Convert.ToInt32(objArray[1]),
                                                CustName = objArray[2],
                                                TotAmt = Convert.ToDecimal(objArray[3]),
                                                TimeStamp = dt
                                            });
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        msg += "Error: Invalid Timestamp of the customer: " + objArray[0] + "\n";

                                        _logger.LogInformation("ParseInputFileData {0}", ex.Message);
                                        _log4net.Debug("ParseInputFileData API " + ex.Message);
                                    }
                                }

                            }
                            counter++;
                        }
                        file.Close();
                    }

                    ret = _repo.SaveData(obj);

                    if (ret)
                    {
                        resp.Status = StatusCode(200);
                        resp.Message = msg;
                        return resp;
                    }
                    else
                    {
                        resp.Status = StatusCode(400);
                        resp.Message = msg != "" ? msg : "Bad request";
                        return resp;
                    }
                }

                resp.Status = StatusCode(400);
                resp.Message = "Bad request";
                return resp;
            }
            catch
            {
                resp.Status = StatusCode(500);
                resp.Message = "Server error";
                return resp;
            }
        }


        [HttpGet]
        [Route("GetData")]
        public ResponseMessage GetData()
        {
            ResponseMessage resp = new ResponseMessage();

            var result = _repo.GetDetails();
            

            if (result != null)
            {
                resp.Status = StatusCode(200);
                resp.Message = "Ok";
                resp.Data = result;
            }
            else
            {
                resp.Status = StatusCode(404);
                resp.Message = "Records not found";

                resp.Data = result;
            }
            return resp;
        }
        private string RemoveBraces(string dt)
        {
            return dt.Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "");
        }
    }
}
