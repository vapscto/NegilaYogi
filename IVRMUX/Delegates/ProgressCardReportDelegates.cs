using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Exam;

using Newtonsoft.Json;


namespace corewebapi18072016.Delegates
{
    public class ProgressCardReportDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/ProgressCardReportFacadeController/" + resource).Result;
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

        public ProgressCardReportDTO Getdetails(ProgressCardReportDTO data)
        {
            ProgressCardReportDTO DTO = new ProgressCardReportDTO();
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
                var response = client.PostAsync("api/ProgressCardReportFacade/Getdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<ProgressCardReportDTO>(product, new JsonSerializerSettings
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

        public ProgressCardReportDTO editdetails(int ID)//Int32 AMA_Id
        {
            ProgressCardReportDTO ProgressCardReportDTO = new ProgressCardReportDTO();
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
                var response = client.GetAsync("api/ProgressCardReportFacade/editdetails/" + ID).Result;             
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    ProgressCardReportDTO = JsonConvert.DeserializeObject<ProgressCardReportDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return ProgressCardReportDTO;
        }

        public ProgressCardReportDTO savedetails(ProgressCardReportDTO data)//Int32 IVRMM_Id
        {
            ProgressCardReportDTO DTO = new ProgressCardReportDTO();
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
                var response = client.PostAsync("api/ProgressCardReportFacade/savedetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<ProgressCardReportDTO>(product, new JsonSerializerSettings
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
        
        public ProgressCardReportDTO validateordernumber(ProgressCardReportDTO data)//Int32 IVRMM_Id
        {
            ProgressCardReportDTO DTO = new ProgressCardReportDTO();
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
                var response = client.PostAsync("api/ProgressCardReportFacade/validateordernumber", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<ProgressCardReportDTO>(product, new JsonSerializerSettings
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

        public ProgressCardReportDTO deactivate(ProgressCardReportDTO data)//Int32 IVRMM_Id
        {
            ProgressCardReportDTO DTO = new ProgressCardReportDTO();
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
                var response = client.PostAsync("api/ProgressCardReportFacade/deactivate", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<ProgressCardReportDTO>(product, new JsonSerializerSettings
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

    }
}
