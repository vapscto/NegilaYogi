using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class ClassSectionAvgDelegates
    {
        public ClassSectionAvgDTO getdetails(ClassSectionAvgDTO id)
        {

            ClassSectionAvgDTO orgdet = null;
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
                var response = client.PostAsync("api/ClassSectionAvgFacade/getdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ClassSectionAvgDTO>(product, new JsonSerializerSettings
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

        public ClassSectionAvgDTO onselectCategory(ClassSectionAvgDTO id)
        {

            ClassSectionAvgDTO orgdet = null;
            string product = "";
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
                var response = client.PostAsync("api/ClassSectionAvgFacade/onselectCategory", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ClassSectionAvgDTO>(product, new JsonSerializerSettings
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


        public ClassSectionAvgDTO onselectclass(ClassSectionAvgDTO id)
        {

            ClassSectionAvgDTO orgdet = null;
            string product = "";
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
                var response = client.PostAsync("api/ClassSectionAvgFacade/onselectclass", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ClassSectionAvgDTO>(product, new JsonSerializerSettings
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

        public ClassSectionAvgDTO onreport(ClassSectionAvgDTO id)
        {

            ClassSectionAvgDTO orgdet = null;
            string product = "";
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
                var response = client.PostAsync("api/ClassSectionAvgFacade/onreport", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<ClassSectionAvgDTO>(product, new JsonSerializerSettings
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
