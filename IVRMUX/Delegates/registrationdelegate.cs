using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class registrationdelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<regis, regis> COM = new CommonDelegate<regis, regis>();
        CommonDelegate<FileDescriptionDTO, FileDescriptionDTO> COMF = new CommonDelegate<FileDescriptionDTO, FileDescriptionDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }

        public regis regdata(regis lo)
        {

            return COM.POSTData(lo, "registrationFacade/");

           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     var myContent = JsonConvert.SerializeObject(lo);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //         lo = JsonConvert.DeserializeObject<regis>(product, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects
           //         });
           //         //lo = JsonConvert.DeserializeObject<regis>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch (Exception e)
           // {
           //     Console.WriteLine(e.Message);
           // }
           // return lo;
        }
        public FileDescriptionDTO alu_regdata(FileDescriptionDTO dto)
        {

            return COMF.POSTData(dto, "registrationFacade/alu_regdata/");

         
        }
        public regis paymentPart(regis lo)
        {

            return COM.POSTData(lo, "registrationFacade/paymentPart/");
            
        }

        public bool getregdata(string username)
        {


            string product="";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(username);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
               // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;
                var response = client.GetAsync("api/registrationFacade/").Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }
            return Convert.ToBoolean(product);
        }
        public bool sendMail(regis r)
        {
            return Convert.ToBoolean(COM.POSTData(r, "registrationFacade/sendmail"));


            //string product = "";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:65140");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(r);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;
            //    var response = client.PostAsync("api/registrationFacade/sendmail", byteContent).Result;

            //    // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);
            //    }
            //}
            //catch
            //{

            //}
            //return Convert.ToBoolean(product);
        }
        public bool ConfirmMail(regis r)
        {

            return Convert.ToBoolean(COM.POSTData(r, "registrationFacade/confirm"));

            //string product = "";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:65140");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(r);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;
            //    var response = client.PostAsync("api/registrationFacade/confirm", byteContent).Result;

            //    // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);
            //    }
            //}
            //catch
            //{

            //}
            //return Convert.ToBoolean(product);
        }
    }
}
