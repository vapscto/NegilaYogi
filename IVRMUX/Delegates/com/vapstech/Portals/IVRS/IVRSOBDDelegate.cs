using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.IVRS;

namespace IVRMUX.Delegates.com.vapstech.Portals.IVRS
{
    public class IVRSOBDDelegate
    {
        public IVRSOBD ivrgetstudetails(IVRSOBD vaue)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var datacon = JsonConvert.SerializeObject(vaue);
                var buff = System.Text.Encoding.UTF8.GetBytes(datacon);
                var bytearraycont = new ByteArrayContent(buff);
                bytearraycont.Headers.ContentType=new MediaTypeHeaderValue("application/json");
                var respon = client.PostAsync("api/IVRSOBDFacade/ivrgetstudetails", bytearraycont).Result;
                if(respon.IsSuccessStatusCode)
                {
                    product = respon.Content.ReadAsStringAsync().Result;
                    vaue = JsonConvert.DeserializeObject<IVRSOBD>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return vaue;
        }

        public IVRSOBD initiatecalls(IVRSOBD vaue)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var datacon = JsonConvert.SerializeObject(vaue);
                var buff = System.Text.Encoding.UTF8.GetBytes(datacon);
                var bytearraycont = new ByteArrayContent(buff);
                bytearraycont.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var respon = client.PostAsync("api/IVRSOBDFacade/initiatecalls", bytearraycont).Result;
                if (respon.IsSuccessStatusCode)
                {
                    product = respon.Content.ReadAsStringAsync().Result;
                    vaue = JsonConvert.DeserializeObject<IVRSOBD>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return vaue;
        }
        public IVRSOBD initiatecallsmobiglitz(IVRSOBD vaue)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {
                var datacon = JsonConvert.SerializeObject(vaue);
                var buff = System.Text.Encoding.UTF8.GetBytes(datacon);
                var bytearraycont = new ByteArrayContent(buff);
                bytearraycont.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var respon = client.PostAsync("api/IVRSOBDFacade/initiatecallsmobiglitz", bytearraycont).Result;
                if (respon.IsSuccessStatusCode)
                {
                    product = respon.Content.ReadAsStringAsync().Result;
                    vaue = JsonConvert.DeserializeObject<IVRSOBD>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return vaue;
        }

        public IVRSOBD getdetails(IVRSOBD orgdet)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(orgdet);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRSOBDFacade/getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<IVRSOBD>(product, new JsonSerializerSettings
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
            return orgdet;
        }
    }
}
