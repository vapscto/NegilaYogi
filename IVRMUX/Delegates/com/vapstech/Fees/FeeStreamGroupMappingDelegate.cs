using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Fees
{
    public class FeeStreamGroupMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FeeStreamGroupMappingDTO, FeeStreamGroupMappingDTO> COMMM = new CommonDelegate<FeeStreamGroupMappingDTO, FeeStreamGroupMappingDTO>();

        //public FeeStreamGroupMappingDTO getData(int id)
        //{
        //    return COMMM.POSTDatafee(id, "FeeStreamGroupMappingFacade/getData/");
        //}

        public FeeStreamGroupMappingDTO getData(FeeStreamGroupMappingDTO data)
        {
            FeeStreamGroupMappingDTO enqdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/FeeStreamGroupMappingFacade/getData/",byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    enqdto = JsonConvert.DeserializeObject<FeeStreamGroupMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return enqdto;
        }

        public FeeStreamGroupMappingDTO saveData(FeeStreamGroupMappingDTO pgmod)
        {
            string pagemoduledata = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(pgmod);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/FeeStreamGroupMappingFacade/saveData/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagemoduledata = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", pagemoduledata);
                    pgmod = JsonConvert.DeserializeObject<FeeStreamGroupMappingDTO>(pagemoduledata, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return pgmod;
        }

        public FeeStreamGroupMappingDTO deactivate(FeeStreamGroupMappingDTO id)
        {
            FeeStreamGroupMappingDTO enqdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/FeeStreamGroupMappingFacade/deactivate/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    using (var sr = new StringReader(product))
                    using (var reader = new JsonTextReader(sr))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        serializer.DateParseHandling = DateParseHandling.DateTime;
                        serializer.TypeNameHandling = TypeNameHandling.Auto;
                        enqdet = serializer.Deserialize<FeeStreamGroupMappingDTO>(reader);
                    }

                    // enqdet = JsonConvert.DeserializeObject<Enq>(product, jsonConve);
                    enqdet = JsonConvert.DeserializeObject<FeeStreamGroupMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateParseHandling = DateParseHandling.DateTime
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdet;
        }


        public FeeStreamGroupMappingDTO EditDetails(int id)
        {
            FeeStreamGroupMappingDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/FeeStreamGroupMappingFacade/Editdetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<FeeStreamGroupMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return orgdet;
        }


    }
}
