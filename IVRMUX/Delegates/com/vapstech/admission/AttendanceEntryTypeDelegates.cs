using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class AttendanceEntryTypeDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AttendanceEntryTypeDTO, AttendanceEntryTypeDTO> COMMM = new CommonDelegate<AttendanceEntryTypeDTO, AttendanceEntryTypeDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/AttendanceEntryTypeFacade/" + resource).Result;
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

        public AttendanceEntryTypeDTO GetAttendanceEnetryTypeData(AttendanceEntryTypeDTO lo)
        {
           // return COMMM.POSTDataADM(SchoolYearWiseStudentDTO, "SectionAllotmentFacade/getAllDetails/");

            return COMMM.POSTDataADM(lo, "AttendanceEntryTypeFacade/Getdetails");

        }

        public AttendanceEntryTypeDTO AttendanceTypeEntryData(AttendanceEntryTypeDTO AttendanceEntryTypeDTO)
        {

            return COMMM.POSTDataADM(AttendanceEntryTypeDTO, "AttendanceEntryTypeFacade/");


        }

        public AttendanceEntryTypeDTO GetSelectedRowDetails(AttendanceEntryTypeDTO ID)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataADM(ID, "AttendanceEntryTypeFacade/GetSelectedRowDetails/");

        }

        public AttendanceEntryTypeDTO AttendanceDeleteEntryTypeDTO(int ID)//Int32 IVRMM_Id
        {
            return COMMM.DeleteDataByIdADM(ID, "AttendanceEntryTypeFacade/AttendanceDeleteEntryTypeDATA/");
            
        }

    }
}
