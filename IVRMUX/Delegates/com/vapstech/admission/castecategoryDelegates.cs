using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class castecategoryDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<castecategoryDTO, castecategoryDTO> COMMM = new CommonDelegate<castecategoryDTO, castecategoryDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/castecategoryFacadeController/" + resource).Result;
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

        public castecategoryDTO GetcastecategoryData(castecategoryDTO lo)
        {
            return COMMM.GETDataADm(lo, "castecategoryFacade/Getdetails");
            

           // castecategoryDTO DTO = new castecategoryDTO();
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
           //     var response = client.GetAsync("api/castecategoryFacade/Getdetails").Result;

           
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);

           //         DTO = JsonConvert.DeserializeObject<castecategoryDTO>(product, new JsonSerializerSettings
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

        public castecategoryDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            return COMMM.GetDataByIdADM(ID, "castecategoryFacade/GetSelectedRowDetails/");

           // castecategoryDTO castecategoryDTO = new castecategoryDTO();
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
           //     var response = client.GetAsync("api/castecategoryFacade/GetSelectedRowDetails/" + ID).Result;             
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //         castecategoryDTO = JsonConvert.DeserializeObject<castecategoryDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch
           // {

           // }
           // return castecategoryDTO;
        }

        public castecategoryDTO castecategoryData(castecategoryDTO castecategoryDTO)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataADM(castecategoryDTO, "castecategoryFacade/");


           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {              

           //     var myContent = JsonConvert.SerializeObject(castecategoryDTO);//IVRMM_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
           //     var response = client.PostAsync("api/castecategoryFacade/", byteContent).Result;
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return castecategoryDTO;
        
        }

        public castecategoryDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.DeleteDataByIdADM(ID, "castecategoryFacade/MasterDeleteModulesDATA/");

           // castecategoryDTO castecategoryDTO = new castecategoryDTO();
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
           //     var response = client.DeleteAsync("api/castecategoryFacade/MasterDeleteModulesDATA/" + ID).Result;     
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return castecategoryDTO;

        }

    }
}
