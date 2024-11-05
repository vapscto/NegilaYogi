using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class RegistrationReportDelegates
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
                HttpResponseMessage response = client.GetAsync("api/ReportProspectusFacade/" + resource).Result;
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
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "RegistrationReportFacade/Getdetails/");
        }


        public WrittenTestMarksBindDataDTO Getdetailsforpre(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "RegistrationReportFacade/Getdetailsforpre/");
        }

        public WrittenTestMarksBindDataDTO get_intial_data(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "RegistrationReportFacade/Get_Intial_data/");
        }

        public WrittenTestMarksBindDataDTO smssend(WrittenTestMarksBindDataDTO data)
        {
            return COMMM.POSTData(data, "RegistrationReportFacade/smssend/");
        }
        public WrittenTestMarksBindDataDTO avtivedeactive(WrittenTestMarksBindDataDTO data)
        {
            return COMMM.POSTData(data, "RegistrationReportFacade/avtivedeactive/");
        }
        public WrittenTestMarksBindDataDTO emailsend(WrittenTestMarksBindDataDTO data)
        {
            return COMMM.POSTData(data, "RegistrationReportFacade/emailsend/");
        }



        public string SendSms(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "success";
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

                var response = client.PostAsync("api/RegistrationReportFacade/SendSms/", byteContent).Result;

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
            string product = "success";
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

                var response = client.PostAsync("api/RegistrationReportFacade/SendMail/", byteContent).Result;

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
            string product = "success";
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

                var response = client.PostAsync("api/RegistrationReportFacade/ExportToExcle/", byteContent).Result;

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

        public WrittenTestMarksBindDataDTO searchprospectus(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "success";
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

                var response = client.PostAsync("api/RegistrationReportFacade/searchprospectus/", byteContent).Result;

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
            return temp;

        }


    }
}
