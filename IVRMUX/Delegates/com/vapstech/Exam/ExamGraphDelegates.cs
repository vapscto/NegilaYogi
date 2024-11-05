using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamGraphDelegates
    {
        public ExamGraphDTO getdetails(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/getdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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

        public ExamGraphDTO onreport(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onreport", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO OnAcdyear(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/OnAcdyear", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO onclasschange(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onclasschange", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO onsectionchange(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onsectionchange", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO onchangeexam(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onchangeexam", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO onchangecategory(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onchangecategory", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
        public ExamGraphDTO onchangesubject(ExamGraphDTO id)
        {

            ExamGraphDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ExamGraphFacade/onchangesubject", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ExamGraphDTO>(product, new JsonSerializerSettings
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
