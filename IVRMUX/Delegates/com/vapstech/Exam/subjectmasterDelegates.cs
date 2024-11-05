
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;


namespace corewebapi18072016.Delegates
{
    public class subjectmasterDelegates
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
                HttpResponseMessage response = client.GetAsync("api/subjectmasterFacadeController/" + resource).Result;
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

        public subjectmasterDTO GetsubjectmasterData(subjectmasterDTO lo)
        {
            subjectmasterDTO DTO = new subjectmasterDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {             

                var myContent = JsonConvert.SerializeObject(lo);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/subjectmasterFacade/Getdetails/",byteContent).Result;

           
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<subjectmasterDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch 
            {

            }
            return DTO;
        }

        public subjectmasterDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            subjectmasterDTO subjectmasterDTO = new subjectmasterDTO();
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
                var response = client.GetAsync("api/subjectmasterFacade/GetSelectedRowDetails/" + ID).Result;             
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    subjectmasterDTO = JsonConvert.DeserializeObject<subjectmasterDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return subjectmasterDTO;
        }

        public subjectmasterDTO subjectmasterData(subjectmasterDTO subjectmasterDTO)//Int32 IVRMM_Id
        {

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {         

                var myContent = JsonConvert.SerializeObject(subjectmasterDTO);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
                var response = client.PostAsync("api/subjectmasterFacade/", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    subjectmasterDTO = JsonConvert.DeserializeObject<subjectmasterDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return subjectmasterDTO;
        
        }

        public subjectmasterDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {
            subjectmasterDTO subjectmasterDTO = new subjectmasterDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {              

                var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
                var response = client.DeleteAsync("api/subjectmasterFacade/MasterDeleteModulesDATA/" + ID).Result;     
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    subjectmasterDTO = JsonConvert.DeserializeObject<subjectmasterDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }
            return subjectmasterDTO;
        }

    }
}
