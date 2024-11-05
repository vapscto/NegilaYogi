using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PreadmissionDTOs;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class TotalStrengthDelegate
    {
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        public TotalStrengthDelegate() { }

       

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        public TotalStrengthDelegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }

        public Adm_M_StudentDTO GetDataById(int mi_id)
        {
            Adm_M_StudentDTO stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(mi_id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/TotalStrengthFacade/getdata/"+mi_id).Result;
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
                var response = client.PostAsync("api/TotalStrengthFacade/getStudData", byteContent).Result;
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
        
        public Adm_M_StudentDTO getsection(Adm_M_StudentDTO stuDTO)
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
                var response = client.PostAsync("api/TotalStrengthFacade/getsection", byteContent).Result;
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

        public Adm_M_StudentDTO getclass(Adm_M_StudentDTO stuDTO)
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
                var response = client.PostAsync("api/TotalStrengthFacade/getclass", byteContent).Result;
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

        public Adm_M_StudentDTO getelective(Adm_M_StudentDTO stuDTO)
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
                var response = client.PostAsync("api/TotalStrengthFacade/getelective", byteContent).Result;
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
