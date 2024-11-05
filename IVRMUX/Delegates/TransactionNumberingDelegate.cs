using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class TransactionNumberingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Master_NumberingDTO, MandatoryFieldsDTO> COMMM = new CommonDelegate<Master_NumberingDTO, MandatoryFieldsDTO>();
        CommonDelegate<Master_NumberingDTO, Master_NumberingDTO> COMM = new CommonDelegate<Master_NumberingDTO, Master_NumberingDTO>();
        public Master_NumberingDTO saveMaster_Numberingdetails(Master_NumberingDTO TnDTO)
        {
            return COMM.POSTData(TnDTO, "TransactionNumberingFacade/");

            // string product = "";
            // HttpClient client = new HttpClient();
            // client.BaseAddress = new Uri("http://localhost:65140/");
            // client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            // //HTTP POST
            // try
            // {
            //     var myContent = JsonConvert.SerializeObject(TnDTO);
            //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


            //     var byteContent = new ByteArrayContent(buffer);
            //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //     var response = client.PostAsync("api/TransactionNumberingFacade/", byteContent).Result;

            //     // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
            //     if (response.IsSuccessStatusCode)
            //     {
            //         product = response.Content.ReadAsStringAsync().Result;
            //         Console.WriteLine("", product);
            //         TnDTO = JsonConvert.DeserializeObject<Master_NumberingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            //     }
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e.Message);
            // }
            // return TnDTO;
        }

        //

        public Master_NumberingDTO Master_NumberingDetails(MandatoryFieldsDTO id)
        {
            return COMMM.POSTDataa(id, "TransactionNumberingFacade/getdetails/");

            // Master_NumberingDTO orgdet = null;
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
            //     var response = client.PostAsync("api/TransactionNumberingFacade/getdetails/", byteContent).Result;

            //     if (response.IsSuccessStatusCode)
            //     {
            //         product = response.Content.ReadAsStringAsync().Result;

            //         orgdet = JsonConvert.DeserializeObject<Master_NumberingDTO>(product, new JsonSerializerSettings
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



        public Master_NumberingDTO deleteRollnoconfig(Master_NumberingDTO id)
        {
            return COMM.POSTDataa(id, "TransactionNumberingFacade/deleteRollnoconfig/");
        }
    }
}
