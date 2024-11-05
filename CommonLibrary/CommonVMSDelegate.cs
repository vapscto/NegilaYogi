using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Options;

namespace CommonLibrary
{
    public class CommonVMSDelegate<T, E> where T : class where E : class
    {

        //private readonly object resource;
        //private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        //private readonly corewebapi18072016.FacadeUrl _config;
        // private static FacadeUrl fdu = new FacadeUrl();

        // empty Constructor, dnt delete please
        public CommonVMSDelegate() { }
        string URLRecruitment = "http://localhost:18123/";
     

     
        public T GETData(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GetDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLRecruitment);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //

        // Delete Data by id
        public T DeleteDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //        
        public T POSTData(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETSEarchData(int id, T dto, string facadeurl)
        {
            string pagedetails;
            T enqdto = null;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(dto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", pagedetails);
                    enqdto = JsonConvert.DeserializeObject<T>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdto;
        }
    }
}