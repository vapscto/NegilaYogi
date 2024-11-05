using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using System.Collections;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class WrittenTestScheduleDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDetailsDTO, StudentDetailsDTO> COMMM = new CommonDelegate<StudentDetailsDTO, StudentDetailsDTO>();
        CommonDelegate<WrittenTestScheduleDTO, WrittenTestScheduleDTO> COMMMM = new CommonDelegate<WrittenTestScheduleDTO, WrittenTestScheduleDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/WrittenTestScheduleFacadeController/" + resource).Result;
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

        public StudentDetailsDTO GetWrittenTestScheduleData(StudentDetailsDTO lo)
        {
            return COMMM.POSTData(lo, "WrittenTestScheduleFacade/Getdetails/");

        
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            return COMMM.GetDataById(ID, "WrittenTestScheduleFacade/GetStudentdetails/");

        }

        public WrittenTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            return COMMMM.GetDataById(ID, "WrittenTestScheduleFacade/GetSelectedRowDetails/");
        }

        public WrittenTestScheduleDTO WrittenTestScheduleData(WrittenTestScheduleDTO WrittenTestScheduleDTO)
        {
            return COMMMM.POSTData(WrittenTestScheduleDTO, "WrittenTestScheduleFacade/");
        }

        public WrittenTestScheduleDTO WrittenTestScheduleDeletesData(int ID)
        {
            return COMMMM.GetDataById(ID, "WrittenTestScheduleFacade/WrittenTestScheduleDeletesData/");
        }



        //public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(int ID,int MID)
        //{
        //    WrittenTestScheduleDTO WrittenTestScheduleDTO = new WrittenTestScheduleDTO();
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:65140/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //   client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    try
        //    {

        //        var myContent = JsonConvert.SerializeObject(ID);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        var response = client.DeleteAsync("api/WrittenTestScheduleFacade/WrittenTestScheduleDeletesStudentData/" + ID + "/"+MID).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return WrittenTestScheduleDTO;

        //}
        public WrittenTestScheduleDTO WrittenTestScheduleDeletesStudentData(WrittenTestScheduleDTO WrittenTestScheduleDTO)
        {
            return COMMMM.POSTData(WrittenTestScheduleDTO, "WrittenTestScheduleFacade/WrittenTestScheduleDeletesStudentData/");
        }

    }

}
