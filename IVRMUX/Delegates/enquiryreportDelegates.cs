using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class enquiryreportDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO> COMMM = new CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO>();
       
       
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/enquiryreportFacade/" + resource).Result;
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


        public WrittenTestMarksBindDataDTO GetData(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "enquiryreportFacade/Getdetails/");
        }


//Added on 13 feb-2016 by sudeep
        public WrittenTestMarksBindDataDTO searchenquiry(WrittenTestMarksBindDataDTO dto)
        {
            return COMMM.POSTData(dto, "enquiryreportFacade/searchenquiry/");
        }             //
        

        public string SendSms(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
         
            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "sucess";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/enquiryreportFacade/SendSms/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<WrittenTestMarksBindDataDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return product;

        }

        public string SendMail(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "sucess";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/enquiryreportFacade/SendMail/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<WrittenTestMarksBindDataDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return product;

        }

        public string ExportToExcle(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "sucess";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/enquiryreportFacade/ExportToExcle/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<WrittenTestMarksBindDataDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return product;

        }


        public WrittenTestMarksBindDataDTO getdetails(WrittenTestMarksBindDataDTO data)
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
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/enquiryreportFacade/getenqyearlist/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<WrittenTestMarksBindDataDTO>(product, new JsonSerializerSettings
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
            return data;
        }

    }
}
