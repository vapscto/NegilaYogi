
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Newtonsoft.Json;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class PushNotifyDelegates
    {
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<PushNotifyDTO, PushNotifyDTO> COMMM = new CommonDelegate<PushNotifyDTO, PushNotifyDTO>();

     

        public PushNotifyDTO Getdetails(PushNotifyDTO data)
        {        
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/PushNotifyFacade/Getdetails", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    data = JsonConvert.DeserializeObject<PushNotifyDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }


        public PushNotifyDTO GetEmployeeDetailsByLeaveYearAndMonth(PushNotifyDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "PushNotifyFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }
        public PushNotifyDTO Getdepartment(PushNotifyDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "PushNotifyFacade/Getdepartment/");
        }
        public PushNotifyDTO get_designation(PushNotifyDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "PushNotifyFacade/get_designation/");
        }
        public PushNotifyDTO get_employee(PushNotifyDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "PushNotifyFacade/get_employee/");
        }
        public PushNotifyDTO savedetail(PushNotifyDTO maspage)
        {
            return COMMM.POSTPORTALData(maspage, "PushNotifyFacade/savedetail/");
        }

      

        public PushNotifyDTO GetClass(PushNotifyDTO data)
        {
            PushNotifyDTO DTO = new PushNotifyDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/PushNotifyFacade/GetClass", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<PushNotifyDTO>(product, new JsonSerializerSettings
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

        public PushNotifyDTO GetSection(PushNotifyDTO data)
        {
            PushNotifyDTO DTO = new PushNotifyDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/PushNotifyFacade/GetSection", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<PushNotifyDTO>(product, new JsonSerializerSettings
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

        public PushNotifyDTO GetStudentDetails(PushNotifyDTO data)
        {
            PushNotifyDTO DTO = new PushNotifyDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:51263/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/PushNotifyFacade/GetStudentDetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<PushNotifyDTO>(product, new JsonSerializerSettings
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
