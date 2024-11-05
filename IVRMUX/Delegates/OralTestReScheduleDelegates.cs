using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class OralTestReScheduleDelegates  
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDetailsDTO, StudentDetailsDTO> COMMM = new CommonDelegate<StudentDetailsDTO, StudentDetailsDTO>();
        CommonDelegate<OralTestScheduleDTO, OralTestScheduleDTO> COMMMM = new CommonDelegate<OralTestScheduleDTO, OralTestScheduleDTO>();
        
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/OralTestReScheduleFacade/" + resource).Result;
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

        public StudentDetailsDTO GetOralTestScheduleData(StudentDetailsDTO lo)
        {
            return COMMM.POSTData(lo, "OralTestReScheduleFacade/Getdetails/");
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            return COMMM.GetDataById(ID, "OralTestReScheduleFacade/GetStudentdetails/");
        }

        public ScheduleReportDTO Getreportdetails(ScheduleReportDTO data)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/OralTestReScheduleFacade/Getreportdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    data = JsonConvert.DeserializeObject<ScheduleReportDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return data;
        }

        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            return COMMMM.GetDataById(ID, "OralTestReScheduleFacade/GetSelectedRowDetails/");
        }

        public OralTestScheduleDTO OralTestScheduleData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            return COMMMM.POSTData(OralTestScheduleDTO, "OralTestReScheduleFacade/");
        }

        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {
            return COMMMM.DeleteDataById(ID, "OralTestReScheduleFacade/OralTestScheduleDeletesData/");
        }

        public OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            return COMMMM.POSTData(OralTestScheduleDTO, "OralTestReScheduleFacade/OralTestScheduleDeletesStudentData/");
        }


    }
}
