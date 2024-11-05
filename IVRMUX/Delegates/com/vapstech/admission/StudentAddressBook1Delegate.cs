using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates


{
    public class StudentAddressBook1Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();

        CommonDelegate<StudentAddressBook1DTO, StudentAddressBook1DTO> _bday = new CommonDelegate<StudentAddressBook1DTO, StudentAddressBook1DTO>();

        public StudentAddressBook1Delegate() { }

        public StudentAddressBook1Delegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }
        public StudentAddressBook1DTO getData(int mi_id)
        {
            return _bday.GetDataByIdADM(mi_id, "StudentAddressBook1Facade/getinitialdata/");
        }
        public StudentAddressBook1DTO yearchange(StudentAddressBook1DTO rtnData)
        {
            return _bday.POSTDataADM(rtnData, "StudentAddressBook1Facade/yearchange/");
        }

        public StudentAddressBook1DTO sectionchange(StudentAddressBook1DTO dto)
        {
            return _bday.POSTDataADM(dto, "StudentAddressBook1Facade/sectinchange/");
        }

        public StudentAddressBook1DTO getdetails(StudentAddressBook1DTO resource)
        {
            return _bday.POSTDataADM(resource, "StudentAddressBook1Facade/getdetails/");
        }

        public string ExportToExcle(StudentAddressBook1DTO StudentAddressBook1DTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            StudentAddressBook1DTO temp = null;
            string product = "sucess";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(StudentAddressBook1DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAddressBook1Facade/ExportToExcle/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return product;

        }



        //public StudentAddressBook1DTO getData(int mi_id)
        //{
        //    StudentAddressBook1DTO rtnData = null;
        //    string product;

        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(mi_id);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.GetAsync("api/StudentAddressBook1Facade/getinitialdata/" + mi_id).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;



        //            rtnData = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return rtnData;

        //}


        //public StudentAddressBook1DTO yearchange(StudentAddressBook1DTO rtnData)
        //{
        //    //StudentAddressBook1DTO rtnData = null;
        //    string product;

        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(rtnData);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentAddressBook1Facade/yearchange/" , byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;



        //            rtnData = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return rtnData;

        //}




        //public StudentAddressBook1DTO sectionchange(StudentAddressBook1DTO dto)
        //{
        //    StudentAddressBook1DTO rtnData = null;
        //    string product;

        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:53497");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(dto);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/StudentAddressBook1Facade/sectinchange/",byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;



        //            rtnData = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return rtnData;

        //}
        //public StudentAddressBook1DTO getdetails(StudentAddressBook1DTO resource)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //     client.BaseAddress = new Uri("http://localhost:53497");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    // HTTP GET
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(resource);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        HttpResponseMessage response = client.PostAsync("api/StudentAddressBook1Facade/getdetails", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            resource = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetBaseException());
        //    }
        //    return resource;
        //}














        public StudentAddressBook1DTO getData1(int mi_id)
        {
            StudentAddressBook1DTO rtnData = null;
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
                var response = client.GetAsync("api/StudentAddressBook1Facade/classchange/" + mi_id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;



                    rtnData = JsonConvert.DeserializeObject<StudentAddressBook1DTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects });


                }
            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return rtnData;

        }


    }
}
