using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace corewebapi18072016.Delegates
{
    public class InstitutionTemplateDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();

        public InstitutionTemplateDelegate() { }
        public InstitutionTemplateDelegate(FacadeUrl config) { _config = config; fdu = config; }

        public CommonDTO getBasicData(int id)
        {
            CommonDTO returnData = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/InstitutionTemplateFacade/getdrpdwns/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    returnData = JsonConvert.DeserializeObject<CommonDTO>(product, new JsonSerializerSettings
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
            return returnData;
        }

        public InstitutionTemplateDTO getEditData(int Id)
        {
            InstitutionTemplateDTO returnData = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(Id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/InstitutionTemplateFacade/geteditdata/" + Id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    returnData = JsonConvert.DeserializeObject<InstitutionTemplateDTO>(product, new JsonSerializerSettings
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
            return returnData;
        }

        public void deleteRec(int id)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/InstitutionTemplateFacade/deleteRec/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        public void DeatciveActive(int id)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/InstitutionTemplateFacade/deactiveactive/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }

        public InstitutionTemplateDTO SaveInstitueTemplate(InstitutionTemplateDTO InstTemp)
        {
            string student;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(InstTemp);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/InstitutionTemplateFacade/", byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    student = response.Content.ReadAsStringAsync().Result;
                    InstTemp = JsonConvert.DeserializeObject<InstitutionTemplateDTO>(student);
                    Console.WriteLine("", student);
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return InstTemp;
        }
    }
}
