using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates


{
    public class StudentAddressBook2Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();

        CommonDelegate<StudentAddressBook2DTO, StudentAddressBook2DTO> _address2 = new CommonDelegate<StudentAddressBook2DTO, StudentAddressBook2DTO>();

        public StudentAddressBook2Delegate() { }
        public StudentAddressBook2Delegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }
        public StudentAddressBook2DTO getData(int mi_id)
        {
            return _address2.GetDataByIdADM(mi_id, "StudentAddressBook2Facade/getinitialdata/");
        }
        public StudentAddressBook2DTO yearchange(StudentAddressBook2DTO rtnData)
        {
            return _address2.POSTDataADM(rtnData, "StudentAddressBook2Facade/yearchange/");
        }
        public StudentAddressBook2DTO sectionchange(StudentAddressBook2DTO mi_id)
        {
            return _address2.POSTDataADM(mi_id, "StudentAddressBook2Facade/sectionchange/");
        }
        public StudentAddressBook2DTO getdetails(StudentAddressBook2DTO resource)
        {
            return _address2.POSTDataADM(resource, "StudentAddressBook2Facade/getdetails/");
        }
        public StudentAddressBook2DTO getdetailsstdemp(StudentAddressBook2DTO resource)
        {
            return _address2.POSTDataADM(resource, "StudentAddressBook2Facade/getdetailsstdemp/");
        }
        public string ExportToExcle(StudentAddressBook2DTO StudentAddressBook2DTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            StudentAddressBook2DTO temp = null;
            string product = "sucess";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(StudentAddressBook2DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAddressBook2Facade/ExportToExcle/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<StudentAddressBook2DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return product;

        }
        public StudentAddressBook2DTO getData33(StudentAddressBook2DTO mi_id)
        {
            StudentAddressBook2DTO rtnData = null;
            string product;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(mi_id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentAddressBook2Facade/sectionchange/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;



                    rtnData = JsonConvert.DeserializeObject<StudentAddressBook2DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return rtnData;

        }
        public StudentAddressBook2DTO getData1(int mi_id)
        {
            StudentAddressBook2DTO rtnData = null;
            string product;

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(mi_id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAddressBook2Facade/classchange/" + mi_id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;



                    rtnData = JsonConvert.DeserializeObject<StudentAddressBook2DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return rtnData;

        }
        public StudentAddressBook2DTO yearchangenew(StudentAddressBook2DTO rtnData)
        {
            return _address2.POSTDataADM(rtnData, "StudentAddressBook2Facade/yearchangenew");
        }
        public StudentAddressBook2DTO classchangenew(StudentAddressBook2DTO rtnData)
        {
            return _address2.POSTDataADM(rtnData, "StudentAddressBook2Facade/classchangenew");
        }
        public StudentAddressBook2DTO sectionchangenew(StudentAddressBook2DTO rtnData)
        {
            return _address2.POSTDataADM(rtnData, "StudentAddressBook2Facade/sectionchangenew");
        }
        public StudentAddressBook2DTO getdetailsnew(StudentAddressBook2DTO rtnData)
        {
            return _address2.POSTDataADM(rtnData, "StudentAddressBook2Facade/getdetailsnew");
        }

    }
}
