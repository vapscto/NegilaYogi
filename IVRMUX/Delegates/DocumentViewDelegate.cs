using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class DocumentViewDelegate
    {

        //public DocumentViewDTO getdetails(int id)
        //{

        //    return COMMM.GetDataById(id, "DocumentViewFacade/getdetailsById/");

        //}
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<DocumentViewDTO, DocumentViewDTO> COMMM = new CommonDelegate<DocumentViewDTO, DocumentViewDTO>();

        public DocumentViewDTO getdetails(DocumentViewDTO id)
        {
           // DocumentViewDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/DocumentViewFacade/getdetails/" , byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    id = JsonConvert.DeserializeObject<DocumentViewDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return id;
        }
        public DocumentViewDTO getDpData(DocumentViewDTO ctry)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/DocumentViewFacade/getDpData/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    ctry = JsonConvert.DeserializeObject<DocumentViewDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return ctry;

        }
        public DocumentViewDTO getdocksonly(DocumentViewDTO ctry)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/DocumentViewFacade/getdocksonly/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    ctry = JsonConvert.DeserializeObject<DocumentViewDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return ctry;

        }


        public DocumentViewDTO StatusGetdetails(DocumentViewDTO id)
        {
            //return COMMM.GETData(lo, "DocumentViewFacade/StatusGetdetails/");

            // DocumentViewDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/DocumentViewFacade/StatusGetdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    id = JsonConvert.DeserializeObject<DocumentViewDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return id;

        }
        public DocumentViewDTO mastersaveDTO(DocumentViewDTO masterDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTData(masterDTO, "DocumentViewFacade/");

        }

        public DocumentViewDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {

            return COMMM.GetDataById(ID, "DocumentViewFacade/GetSelectedRowDetails/");

        }

        public DocumentViewDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.GetDataById(ID, "DocumentViewFacade/MasterDeleteModulesDATA/");

        }
    }
}
