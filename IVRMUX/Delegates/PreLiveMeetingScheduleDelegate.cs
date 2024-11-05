using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates
{
    public class PreLiveMeetingScheduleDelegate 
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LiveMeetingScheduleDTO, LiveMeetingScheduleDTO> COMMM = new CommonDelegate<LiveMeetingScheduleDTO, LiveMeetingScheduleDTO>();


        public LiveMeetingScheduleDTO getalldetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "/getalldetails/");
        }
        public LiveMeetingScheduleDTO getempdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getempdetails/");
        }
        public LiveMeetingScheduleDTO getstudentdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getstudentdetails/");
        }
        public LiveMeetingScheduleDTO onstartmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/onstartmeeting/");
        }
        public LiveMeetingScheduleDTO endmainmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/endmainmeeting/");
        }
        public LiveMeetingScheduleDTO saveremarks(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/saveremarks/");
        }
        public LiveMeetingScheduleDTO getclass(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getclass/");
        }
        public LiveMeetingScheduleDTO ondatechange(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/ondatechange/");
        }
        public LiveMeetingScheduleDTO onschedulecahnge(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/onschedulecahnge/");
        }
        public LiveMeetingScheduleDTO ondatechangestudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/ondatechangestudent/");
        }
        public LiveMeetingScheduleDTO endmainmeetingstudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/endmainmeetingstudent/");
        }
        public LiveMeetingScheduleDTO joinmeetingStudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/joinmeetingStudent/");
        }
        public LiveMeetingScheduleDTO joinmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/joinmeeting/");
        }
        public LiveMeetingScheduleDTO getsection(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getsection/");
        }
        public LiveMeetingScheduleDTO savedata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/savedata/");
        }
        public LiveMeetingScheduleDTO editdata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/editdata/");
        }
        public LiveMeetingScheduleDTO checkduplicate(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/checkduplicate/");
        }
        public LiveMeetingScheduleDTO getsubject(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getsubject/");
        }
      
        public LiveMeetingScheduleDTO deactive(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/deactive/");
        }

        //REPORT
        public LiveMeetingScheduleDTO getschrptdetailsprofile(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getschrptdetailsprofile/");
        }
        public LiveMeetingScheduleDTO getschrptdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getschrptdetails/");
        }
        public LiveMeetingScheduleDTO getstudentprofiledata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getstudentprofiledata/");
        }
        public LiveMeetingScheduleDTO getschedulereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getschedulereport/");
        }
        public LiveMeetingScheduleDTO getstaffprofilereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getstaffprofilereport/");
        }
        public LiveMeetingScheduleDTO getstudentprofilereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTData(data, "PreLiveMeetingScheduleFacade/getstudentprofilereport/");
        }
        
    }
}
