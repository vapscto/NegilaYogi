using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class ClassWiseTTDelegate
    {

        public TTClassWiseTTDTO getrpt(TTClassWiseTTDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClassWiseTTFacade/getrpt", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TTClassWiseTTDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }


        //}
        public TTClassWiseTTDTO getdetails(int id)
        {
            TTClassWiseTTDTO categorypage = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/ClassWiseTTFacade/getdetails/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TTClassWiseTTDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return categorypage;
        }

        public TTClassWiseTTDTO getclass_catg(TTClassWiseTTDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ClassWiseTTFacade/getclass_catg", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<TTClassWiseTTDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }
    }
}
