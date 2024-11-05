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
    public class intimationscheduleDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentDetailsDTO, StudentDetailsDTO> COMMM = new CommonDelegate<StudentDetailsDTO, StudentDetailsDTO>();
        CommonDelegate<OralTestScheduleDTO, OralTestScheduleDTO> COMMMM = new CommonDelegate<OralTestScheduleDTO, OralTestScheduleDTO>();
        CommonDelegate<CommonDTO, CommonDTO> COMM = new CommonDelegate<CommonDTO, CommonDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/intimationscheduleFacadeController/" + resource).Result;
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
            return COMMM.POSTData(lo, "intimationscheduleFacade/Getdetails/");
        }

        public StudentDetailsDTO GetSelectedStudentData(int ID)
        {
            return COMMM.GetDataById(ID, "intimationscheduleFacade/GetStudentdetails/");
        }

        public OralTestScheduleDTO GetSelectedRowDetails(int ID)
        {
            return COMMMM.GetDataById(ID, "intimationscheduleFacade/GetSelectedRowDetails/");
        }

        public OralTestScheduleDTO OralTestScheduleData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            return COMMMM.POSTData(OralTestScheduleDTO, "intimationscheduleFacade/");
        }

        public OralTestScheduleDTO OralTestScheduleDeletesData(int ID)
        {
            return COMMMM.DeleteDataById(ID, "intimationscheduleFacade/OralTestScheduleDeletesData/");
        }

        public OralTestScheduleDTO OralTestScheduleDeletesStudentData(OralTestScheduleDTO OralTestScheduleDTO)
        {
            return COMMMM.POSTData(OralTestScheduleDTO, "intimationscheduleFacade/OralTestScheduleDeletesStudentData/");
        }
        public CommonDTO getStudentOnSearchFilter(CommonDTO cdto)
        {
            return COMM.POSTData(cdto, "intimationscheduleFacade/getdataonsearchfilter/");
        }

        }
}
