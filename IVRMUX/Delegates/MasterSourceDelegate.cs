using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class MasterSourceDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterSourceDTO, MasterSourceDTO> COMMM = new CommonDelegate<MasterSourceDTO, MasterSourceDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
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

        public MasterSourceDTO savedetails(MasterSourceDTO maspage)
        {
            return COMMM.POSTData(maspage, "MasterSourceFacade/");

           // string product;
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     //var pairs = new List<KeyValuePair<string, string>>
           //     //{
           //     //    new KeyValuePair<string, string>("username", lo.username ),
           //     //    new KeyValuePair<string, string>("password", lo.password )
           //     //};
           //     //var content = new FormUrlEncodedContent(pairs);
           //     //var response = client.PostAsJsonAsync("api/PreadmissionFacade/", content).Result;

           //     var myContent = JsonConvert.SerializeObject(maspage);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.PostAsync("api/MasterSourceFacade/", byteContent).Result;

           //     // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //         maspage = JsonConvert.DeserializeObject<MasterSourceDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch
           // {

           // }
           // return maspage;
        }

        public MasterSourceDTO deleterec(int id)
        {

            return COMMM.GetDataById(id, "MasterSourceFacade/deletedetails/");


           // MasterSourceDTO enqdto = null;
           // string product;
           // Array[] dropDownArray = new Array[2];
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     var myContent = JsonConvert.SerializeObject(id);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.DeleteAsync("api/MasterSourceFacade/deletedetails/" + id).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;

           //         enqdto = JsonConvert.DeserializeObject<MasterSourceDTO>(product, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects
           //         });

           //         enqdto = JsonConvert.DeserializeObject<MasterSourceDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch (Exception ee)
           // {
           //     Console.WriteLine(ee.Message);
           // }
           // // return output;
           // return enqdto;
        }

        public MasterSourceDTO getdetails(int id)
        {
            return COMMM.GetDataById(id, "MasterSourceFacade/getdetails/");

           // MasterSourceDTO orgdet = null;
           // string product;
           // Array[] dropDownArray = new Array[2];
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     var myContent = JsonConvert.SerializeObject(id);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.GetAsync("api/MasterSourceFacade/getdetails/" + id).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;

           //         orgdet = JsonConvert.DeserializeObject<MasterSourceDTO>(product, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects
           //         });
           //     }
           // }
           // catch (Exception ee)
           // {
           //     Console.WriteLine(ee.Message);
           // }
           // // return output;
           // return orgdet;
        }

        public MasterSourceDTO getpagedetails(int id)
        {

            return COMMM.GetDataById(id, "MasterSourceFacade/getpagedetails/");

           // MasterSourceDTO pageedit = null;
           // string pagedetails;
           // Array[] dropDownArray = new Array[2];
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     var myContent = JsonConvert.SerializeObject(id);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.GetAsync("api/MasterSourceFacade/getpagedetails/" + id).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         pagedetails = response.Content.ReadAsStringAsync().Result;

           //         pageedit = JsonConvert.DeserializeObject<MasterSourceDTO>(pagedetails, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects
           //         });
           //     }
           // }
           // catch (Exception ee)
           // {
           //     Console.WriteLine(ee.Message);
           // }
           // // return output;
           // return pageedit;
        }

        public MasterSourceDTO getsearchdata(int data, MasterSourceDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterSourceFacade/1/");

           // MasterSourceDTO pageedit = null;
           // string pagedetails;
           // Array[] dropDownArray = new Array[2];
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:65140/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
           //     var myContent = JsonConvert.SerializeObject(dataa);
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
           //     var response = client.PostAsync("api/MasterSourceFacade/1/" , byteContent).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         pagedetails = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", pagedetails);
           //         dataa = JsonConvert.DeserializeObject<MasterSourceDTO>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
           //     }
           // }
           // catch (Exception ee)
           // {
           //     Console.WriteLine(ee.Message);
           // }
           // // return output;
           // return dataa;
        }
    }
}
