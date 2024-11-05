using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MobileApp.Delegates
{
    public class IVRSDelegates
    {
        public JsonResult getdata(IVRSDTO data)
        {
           
            string product="";
            string status = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRSFacade/getdata/" ,byteContent).Result;
                status = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                 //   var status = new {status=response.StatusCode };
                 //   product += status;
                    data = JsonConvert.DeserializeObject<IVRSDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return new JsonResult(new { status = status, JsonData = JsonConvert.DeserializeObject(product) });
        }


        public JsonResult getbranch(IVRSDTO data)
        {
            string product = "";
            string status = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRSFacade/getbranch/", byteContent).Result;
                status = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<IVRSDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return new JsonResult(new { status = status, JsonData = JsonConvert.DeserializeObject(product) });
        }

        public JsonResult updatecredits(IVRSDTO data)
        {

            string product = "";
            string status = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRSFacade/updatecredits/", byteContent).Result;
                status = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    //   var status = new {status=response.StatusCode };
                    //   product += status;
                    data = JsonConvert.DeserializeObject<IVRSDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return new JsonResult(new { status = status, JsonData = JsonConvert.DeserializeObject(product) });
        }
        public JsonResult UpdateMobile(IVRSDTO data)
        {

            string product = "";
            string status = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/IVRSFacade/UpdateMobile/", byteContent).Result;
                status = response.StatusCode.ToString();
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    //   var status = new {status=response.StatusCode };
                    //   product += status;
                    data = JsonConvert.DeserializeObject<IVRSDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return new JsonResult(new { status = status, JsonData = JsonConvert.DeserializeObject(product) });
        }
    }
}
