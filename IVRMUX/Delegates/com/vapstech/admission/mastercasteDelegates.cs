using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class mastercasteDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<mastercasteDTO, mastercasteDTO> COMMM = new CommonDelegate<mastercasteDTO, mastercasteDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/mastercasteFacadeController/" + resource).Result;
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

        public mastercasteDTO GetmastercasteData(mastercasteDTO lo)
        {
            return COMMM.POSTDataADM(lo, "mastercasteFacade/Getdetails");

           // mastercasteDTO DTO = new mastercasteDTO();
           // string product;
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {             

           //     var myContent = JsonConvert.SerializeObject(lo);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.PostAsync("api/mastercasteFacade/Getdetails", byteContent).Result;

           
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);

           //         DTO = JsonConvert.DeserializeObject<mastercasteDTO>(product, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects
           //         });
           //     }
           // }
           // catch 
           // {

           // }
           // return DTO;
        }

        public mastercasteDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {

            return COMMM.GetDataByIdADM(ID, "mastercasteFacade/GetSelectedRowDetails/");

           // mastercasteDTO mastercasteDTO = new mastercasteDTO();
           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");   
           // try
           // { 
           //     var myContent = JsonConvert.SerializeObject(ID);//AMA_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");       
           //     var response = client.GetAsync("api/mastercasteFacade/GetSelectedRowDetails/" + ID).Result;             
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //         mastercasteDTO = JsonConvert.DeserializeObject<mastercasteDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch
           // {

           // }
           // return mastercasteDTO;
        }

        public mastercasteDTO mastercasteData(mastercasteDTO mastercasteDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTDataADM(mastercasteDTO, "mastercasteFacade/");

           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {              

           //     var myContent = JsonConvert.SerializeObject(mastercasteDTO);//IVRMM_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
           //     var response = client.PostAsync("api/mastercasteFacade/", byteContent).Result;
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return mastercasteDTO;
        
        }

        public mastercasteDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.DeleteDataByIdADM(ID, "mastercasteFacade/MasterDeleteModulesDATA/");

           // mastercasteDTO mastercasteDTO = new mastercasteDTO();
           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
               

           //     var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
           //     var response = client.DeleteAsync("api/mastercasteFacade/MasterDeleteModulesDATA/" + ID).Result;     
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return mastercasteDTO;

        }

    }
}
