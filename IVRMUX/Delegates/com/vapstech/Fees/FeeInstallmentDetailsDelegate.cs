using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class FeeInstallmentDetailsDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();

        public FeeInstallmentDetailsDelegate() { }

        public FeeInstallmentDetailsDelegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }

       
    
        public FeeInstallmentsDetailsDTO getdata(FeeInstallmentsDetailsDTO data)
        {
            //FeeInstallmentsDetailsDTO feesinstdet = null;
            //string product;
            //Array[] dropDownArray = new Array[2];
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:49540/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(data);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/FeeInstallmentDetailsFacade/getdetails/" ,byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;

            //        feesinstdet = JsonConvert.DeserializeObject<FeeInstallmentsDetailsDTO>(product, new JsonSerializerSettings
            //        {
            //            TypeNameHandling = TypeNameHandling.Objects
            //        });
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            //return feesinstdet;


            FeeInstallmentsDetailsDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/FeeInstallmentDetailsFacade/getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<FeeInstallmentsDetailsDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;




        }
    }
}
