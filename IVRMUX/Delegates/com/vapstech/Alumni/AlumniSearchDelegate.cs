using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class AlumniSearchDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AlumniStudentDTO, AlumniStudentDTO> COMMM = new CommonDelegate<AlumniStudentDTO, AlumniStudentDTO>();
        public AlumniStudentDTO getData(AlumniStudentDTO stu)
        {

            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54176/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(stu);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/AlumniSearchFacade/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    stu = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }

        public AlumniStudentDTO GetClassWiseDailyAttendanceData(AlumniStudentDTO enqdto)
        {
            return COMMM.POSTDataAlumni(enqdto, "AlumniSearchFacade/Getdetails/");
        }

        public AlumniStudentDTO Getdetailsreport(AlumniStudentDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            AlumniStudentDTO temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54176/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/AlumniSearchFacade/Getdetailsreport/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return temp;

        }

        public AlumniStudentDTO getstate(AlumniStudentDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            AlumniStudentDTO temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54176/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/AlumniSearchFacade/getstate/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return temp;

        }

        public AlumniStudentDTO getdistrict(AlumniStudentDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            AlumniStudentDTO temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:54176/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/AlumniSearchFacade/getdistrict/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<AlumniStudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return temp;

        }
    }
}
