using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class ProspectusDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ProspectusDTO, ProspectusDTO> COMMM = new CommonDelegate<ProspectusDTO, ProspectusDTO>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<CityDTO, CityDTO> COMM = new CommonDelegate<CityDTO, CityDTO>();
        CommonDelegate<Enq, searchEnquiryDTO> COM = new CommonDelegate<Enq, searchEnquiryDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();

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
        public ProspectusDTO savedetails(ProspectusDTO org)
        {
            return COMMM.POSTData(org, "ProspectusFacade/");

        }
        public ProspectusDTO getcountrydata(ProspectusDTO data)
        {

            return COMMM.POSTData(data, "ProspectusFacade/loaddata/");

         
        }

        public StateDTO enqdatacountrydrp(int id)
        {

            return COMMMM.GetDataById(id, "ProspectusFacade/getorganisationcontroller/");

         
        }

        public CityDTO cityfill(int id)
        {
            return COMM.GetDataById(id, "ProspectusFacade/getorganisationstatecontroller/");

          
        }


        public ProspectusDTO deleterec(ProspectusDTO enqdto)
        {

            return COMMM.POSTData(enqdto, "ProspectusFacade/deletedetails/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTData(response, "ProspectusFacade/getpaymentresponse/");

        }

        public ProspectusDTO prospdet(int id)
        {
            return COMMM.GetDataById(id, "ProspectusFacade/getdetails/");

           
        }
        public Enq enqDetails(searchEnquiryDTO id)
        {

            return COM.POSTDataa(id, "ProspectusFacade/getEnquiry/");


           // Enq enqdet = null;
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
           //     var response = client.PostAsync("api/ProspectusFacade/getEnquiry/",byteContent).Result;

           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;


           //         using (var sr = new StringReader(product))
           //         using (var reader = new JsonTextReader(sr))
           //         {
           //             JsonSerializer serializer = new JsonSerializer();
           //             serializer.DateParseHandling = DateParseHandling.DateTime;
           //             serializer.TypeNameHandling = TypeNameHandling.Auto;
           //             enqdet  = serializer.Deserialize<Enq>(reader);
           //         }

           //         // enqdet = JsonConvert.DeserializeObject<Enq>(product, jsonConve);
           //         enqdet = JsonConvert.DeserializeObject<Enq>(product, new JsonSerializerSettings
           //         {
           //             TypeNameHandling = TypeNameHandling.Objects,
           //             DateParseHandling = DateParseHandling.DateTime
           //         });
           //     }
           // }
           // catch (Exception ee)
           // {
           //     Console.WriteLine(ee.Message);
           // }
           // // return output;
           // return enqdet;
        }
        public ProspectusDTO searchByColumn(ProspectusDTO org)
        {
            return COMMM.POSTData(org, "ProspectusFacade/searchByColumn");

        }

        public ProspectusDTO downloadProspectus(int miId)
        {
            return COMMM.GetDataById(miId, "ProspectusFacade/getFilePath/");
        }


    }
}
