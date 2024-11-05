using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;


namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class SRKVSStudyCertificateDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<SRKVSStudycertificateDTO, SRKVSStudycertificateDTO> _comm = new CommonDelegate<SRKVSStudycertificateDTO, SRKVSStudycertificateDTO>();
        public SRKVSStudycertificateDTO getdetails(int id)
        {
            SRKVSStudycertificateDTO ads = null;
            string activatedeac;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/SRKVSStudyCertificateFacade/getdata/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    activatedeac = response.Content.ReadAsStringAsync().Result;

                    ads = JsonConvert.DeserializeObject<SRKVSStudycertificateDTO>(activatedeac, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ads;
        }

        public SRKVSStudycertificateDTO getstudlist(SRKVSStudycertificateDTO student)
        {
            SRKVSStudycertificateDTO ads = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(student);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/SRKVSStudyCertificateFacade/getS/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    ads = JsonConvert.DeserializeObject<SRKVSStudycertificateDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ads;
        }

        public SRKVSStudycertificateDTO GetStudDataById(SRKVSStudycertificateDTO stuDTO)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(stuDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/SRKVSStudyCertificateFacade/getStudData", byteContent).Result;
                //

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<SRKVSStudycertificateDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public SRKVSStudycertificateDTO onacademicyearchange(SRKVSStudycertificateDTO data)
        {
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/SRKVSStudyCertificateFacade/onacademicyearchange", byteContent).Result;
                //

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<SRKVSStudycertificateDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public SRKVSStudycertificateDTO searchfilter(SRKVSStudycertificateDTO data)
        {
            return _comm.POSTDataADM(data, "SRKVSStudyCertificateFacade/searchfilter/");
        }
        public SRKVSStudycertificateDTO Studdetailsconduct(SRKVSStudycertificateDTO data)
        {
            return _comm.POSTDataADM(data, "SRKVSStudyCertificateFacade/Studdetailsconduct/");
        }

    }
}



