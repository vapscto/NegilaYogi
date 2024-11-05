using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates
{
    public class AdmissionRegisterDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/AdmissionRegisterFacadeController/" + resource).Result;
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

        public SchoolYearWiseStudentDTO GetAdmissionRegisterData(SchoolYearWiseStudentDTO lo)
        {
            SchoolYearWiseStudentDTO DTO = new SchoolYearWiseStudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {             

                var myContent = JsonConvert.SerializeObject(lo);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/AdmissionRegisterFacade/Getdetails/", byteContent).Result;

           
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<SchoolYearWiseStudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch 
            {

            }
            return DTO;
        }

        public SchoolYearWiseStudentDTO Getdetailsreport(SchoolYearWiseStudentDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            SchoolYearWiseStudentDTO temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/AdmissionRegisterFacade/Getdetailsreport/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<SchoolYearWiseStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {

            }


            return temp;

        }

        public SchoolYearWiseStudentDTO getclass(SchoolYearWiseStudentDTO stuDTO)
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
                var response = client.PostAsync("api/AdmissionRegisterFacade/getclass", byteContent).Result;
                //

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<SchoolYearWiseStudentDTO>(product, new JsonSerializerSettings
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
        public castecategoryDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {
            castecategoryDTO castecategoryDTO = new castecategoryDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");   
            try
            { 
                var myContent = JsonConvert.SerializeObject(ID);//AMA_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");       
                var response = client.GetAsync("api/AdmissionRegisterFacadeController/GetSelectedRowDetails/" + ID).Result;             
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    castecategoryDTO = JsonConvert.DeserializeObject<castecategoryDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return castecategoryDTO;
        }

        public castecategoryDTO castecategoryData(castecategoryDTO castecategoryDTO)//Int32 IVRMM_Id
        {

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {              

                var myContent = JsonConvert.SerializeObject(castecategoryDTO);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
                var response = client.PostAsync("api/AdmissionRegisterFacadeController/", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return castecategoryDTO;
        
        }

        public castecategoryDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {
            castecategoryDTO castecategoryDTO = new castecategoryDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
               

                var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
                var response = client.DeleteAsync("api/AdmissionRegisterFacadeController/MasterDeleteModulesDATA/" + ID).Result;     
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return castecategoryDTO;

        }
        //public School_M_Section getsec(int id)
        //{
        //    School_M_Section enqdto = null;
        //    string product;
        //    Array[] dropDownArray = new Array[2];
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:65140/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(id);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.GetAsync("api/ClassWiseDailyAttendanceFacadeController/getenquirycontroller/" + id).Result;

        //        // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;

        //            enqdto = JsonConvert.DeserializeObject<School_M_Section>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });

        //            // EnqDTO deserializedProduct = JsonConvert.DeserializeObject<EnqDTO>(output);

        //            //enqdto = await response.Content.ReadAsAsync<EnqDTO>();
        //            //enqdto = (EnqDTO)JsonConvert.DeserializeObject<EnqDTO>(product);

        //            //var deserializedObject = JsonConvert.DeserializeObject<EnqDTO>(product, new JsonSerializerSettings
        //            //{
        //            //    TypeNameHandling = TypeNameHandling.Objects
        //            //});

        //            //enqdto = JsonConvert.DeserializeObject<EnqDTO>(product);

        //            //var obj = JsonConvert.DeserializeObject<EnqDTO>(product);

        //            //dropDownArray = product.ToArray();
        //            // Console.WriteLine("", product);
        //        }
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    // return output;
        //    return enqdto;
        //}
    }
}
