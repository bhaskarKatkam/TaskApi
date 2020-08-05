using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
         static async Task Main(string[] args)
        {

            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine("Please enter input file and minimum sales amount.");
                    return;
                }

                string inputFile = args[0];
                decimal miniamt = Convert.ToDecimal(args[1]);

                if (File.Exists(inputFile))
                {
                    using (var form = new MultipartFormDataContent())
                    using (var stream = File.OpenRead(inputFile))
                       
                    
                    using (var streamContent = new StreamContent(stream))
                    {
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                form.Add(streamContent, "inputFile", Path.GetFileName(inputFile));
                                var strTask = await client.PostAsync("http://localhost:5000/api/DataParser/ParseInputFileData?minSalesAmt="+miniamt, form);
                                var resobj = strTask.Content.ReadAsStringAsync();

                                Console.WriteLine("Read data\n");
                                Console.WriteLine("{0}", resobj.Result);

                                Console.WriteLine("========================================\n");

                                var strTask1 = await client.GetAsync("http://localhost:5000/api/DataParser/GetData");
                                var resobj1 = strTask1.Content.ReadAsStringAsync();

                                Console.WriteLine("Get data\n");
                                Console.WriteLine("{0}", resobj1.Result);
                                Console.ReadKey();
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine("{0}", ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Please enter input file and minimum sales amount.");
                }

            }
            catch
            {
                //return null;
            }
        }
    }
}
