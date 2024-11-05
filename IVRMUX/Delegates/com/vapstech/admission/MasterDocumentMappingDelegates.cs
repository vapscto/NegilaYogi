using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;


namespace corewebapi18072016.com.vaps.admission.Delegates
{
    public class MasterDocumentMappingDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:57606/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/MasterDocumentMappingFacade/" + resource).Result;
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

        public MasterDocumentMappingDTO Getdetails(MasterDocumentMappingDTO lo)
        {
            //AttendanceEntryTypeDTO DTO;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(lo);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/MasterDocumentMappingFacade/Getdetails").Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    lo = JsonConvert.DeserializeObject<MasterDocumentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return lo;
        }

        public MasterDocumentMappingDTO SaveData(MasterDocumentMappingDTO MasterDocumentMappingDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(MasterDocumentMappingDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterDocumentMappingFacade/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterDocumentMappingDTO = JsonConvert.DeserializeObject<MasterDocumentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return MasterDocumentMappingDTO;

        }

        public MasterDocumentMappingDTO GetSelectedRowDetails(int ID)//Int32 IVRMM_Id
        {
            MasterDocumentMappingDTO MasterDocumentMappingDTO = new MasterDocumentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                //var pairs = new List<KeyValuePair<string, string>>
                //{
                //    new KeyValuePair<string, string>("username", lo.username ),
                //    new KeyValuePair<string, string>("password", lo.password )
                //};
                //var content = new FormUrlEncodedContent(pairs);
                //var response = client.PostAsJsonAsync("api/PreadmissionFacade/", content).Result;

                var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/MasterDocumentMappingFacade/GetSelectedRowDetails/" + ID).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterDocumentMappingDTO = JsonConvert.DeserializeObject<MasterDocumentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return MasterDocumentMappingDTO;

        }

        public MasterDocumentMappingDTO DeleteData(int ID)//Int32 IVRMM_Id
        {
            MasterDocumentMappingDTO MasterDocumentMappingDTO = new MasterDocumentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.DeleteAsync("api/MasterDocumentMappingFacade/DeleteEntry/" + ID).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return MasterDocumentMappingDTO;

        }

    }
}
