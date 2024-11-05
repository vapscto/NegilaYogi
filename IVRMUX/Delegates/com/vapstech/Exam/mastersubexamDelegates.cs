
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;


namespace corewebapi18072016.Delegates
{
    public class mastersubexamDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
       

        public mastersubexamDTO Getdetails(mastersubexamDTO data)
        {
            mastersubexamDTO DTO = new mastersubexamDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/mastersubexamFacade/Getdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<mastersubexamDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }

        public mastersubexamDTO editdeatils(int ID)//Int32 AMA_Id
        {
            mastersubexamDTO mastersubexamDTO = new mastersubexamDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");   
            try
            { 
                var myContent = JsonConvert.SerializeObject(ID);//AMA_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");       
                var response = client.GetAsync("api/mastersubexamFacade/editdeatils/" + ID).Result;             
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    mastersubexamDTO = JsonConvert.DeserializeObject<mastersubexamDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return mastersubexamDTO;
        }

        public mastersubexamDTO savedetails(mastersubexamDTO data)//Int32 IVRMM_Id
        {
            mastersubexamDTO DTO = new mastersubexamDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {              

                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
                var response = client.PostAsync("api/mastersubexamFacade/savedetails", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    DTO = JsonConvert.DeserializeObject<mastersubexamDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    //Console.WriteLine("", product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }
        public mastersubexamDTO validateordernumber(mastersubexamDTO data)//Int32 IVRMM_Id
        {
            mastersubexamDTO DTO = new mastersubexamDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/mastersubexamFacade/validateordernumber", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    DTO = JsonConvert.DeserializeObject<mastersubexamDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    //Console.WriteLine("", product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }

        public mastersubexamDTO deactivate(mastersubexamDTO data)//Int32 IVRMM_Id
        {
            mastersubexamDTO DTO = new mastersubexamDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/mastersubexamFacade/deactivate", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    DTO = JsonConvert.DeserializeObject<mastersubexamDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    //Console.WriteLine("", product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;

        }

    }
}
