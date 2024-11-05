
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;


namespace corewebapi18072016.Delegates
{
    public class HHSReport_PreNursDelegates
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
                HttpResponseMessage response = client.GetAsync("api/HHSReport_PreNursFacadeController/" + resource).Result;
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

        public HHSReport_PreNursDTO Getdetails(HHSReport_PreNursDTO DTO)
        {
            
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {             

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/HHSReport_PreNursFacade/Getdetails", byteContent).Result;

           
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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


        public HHSReport_PreNursDTO getclass(HHSReport_PreNursDTO data)
        {
            HHSReport_PreNursDTO DTO = new HHSReport_PreNursDTO();
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
                var response = client.PostAsync("api/HHSReport_PreNursFacade/getclass", byteContent).Result;


                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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

        public HHSReport_PreNursDTO Getsection(HHSReport_PreNursDTO data)
        {
            HHSReport_PreNursDTO DTO = new HHSReport_PreNursDTO();
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
                var response = client.PostAsync("api/HHSReport_PreNursFacade/Getsection", byteContent).Result;


                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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

        public HHSReport_PreNursDTO GetAttendence(HHSReport_PreNursDTO data)
        {
            HHSReport_PreNursDTO DTO = new HHSReport_PreNursDTO();
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
                var response = client.PostAsync("api/HHSReport_PreNursFacade/GetAttendence", byteContent).Result;


                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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

        public HHSReport_PreNursDTO GetIndividualAttendence(HHSReport_PreNursDTO data)
        {
            HHSReport_PreNursDTO DTO = new HHSReport_PreNursDTO();
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
                var response = client.PostAsync("api/HHSReport_PreNursFacade/GetIndividualAttendence", byteContent).Result;


                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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


        public HHSReport_PreNursDTO savedetails(HHSReport_PreNursDTO data)//Int32 IVRMM_Id
        {
            HHSReport_PreNursDTO DTO = new HHSReport_PreNursDTO();
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
                var response = client.PostAsync("api/HHSReport_PreNursFacade/savedetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<HHSReport_PreNursDTO>(product, new JsonSerializerSettings
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
