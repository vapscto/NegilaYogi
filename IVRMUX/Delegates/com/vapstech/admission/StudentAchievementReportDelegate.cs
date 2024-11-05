using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;


namespace corewebapi18072016.Delegates
{
    public class StudentAchievementReportDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        public Adm_M_StudentDTO getdetails(Adm_M_StudentDTO data2)
        {
            Adm_M_StudentDTO ads = null;
            string activatedeac;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data2);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAchievementReportFacade/getdata/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    activatedeac = response.Content.ReadAsStringAsync().Result;
                    
                    ads = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(activatedeac, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });
                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ads;
        }

        public Adm_M_StudentDTO stdnamesyear(Adm_M_StudentDTO data2)
        {
            string activatedeac;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data2);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAchievementReportFacade/stdnamesyear/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    activatedeac = response.Content.ReadAsStringAsync().Result;

                    data2 = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(activatedeac, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data2;
        }

        public Adm_M_StudentDTO GetStudDataById(Adm_M_StudentDTO stuDTO)
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
                var response = client.PostAsync("api/StudentAchievementReportFacade/getStudData", byteContent).Result;
                //

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
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
    }
}
