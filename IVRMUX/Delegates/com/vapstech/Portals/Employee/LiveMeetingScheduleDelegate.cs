using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class LiveMeetingScheduleDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<LiveMeetingScheduleDTO, LiveMeetingScheduleDTO> COMMM = new CommonDelegate<LiveMeetingScheduleDTO, LiveMeetingScheduleDTO>();


        public LiveMeetingScheduleDTO getalldetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getalldetails/");
        }
        public LiveMeetingScheduleDTO getempdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getempdetails/");
        }
        public LiveMeetingScheduleDTO getstudentdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getstudentdetails/");
        }
        public LiveMeetingScheduleDTO onstartmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/onstartmeeting/");
        }
        public LiveMeetingScheduleDTO endmainmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/endmainmeeting/");
        }
        public LiveMeetingScheduleDTO getclass(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getclass/");
        }
        public LiveMeetingScheduleDTO ondatechange(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/ondatechange/");
        }
        public LiveMeetingScheduleDTO ondatechangestudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/ondatechangestudent/");
        }
        public LiveMeetingScheduleDTO endmainmeetingstudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/endmainmeetingstudent/");
        }
        public LiveMeetingScheduleDTO joinmeetingStudent(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/joinmeetingStudent/");
        }
        public LiveMeetingScheduleDTO joinmeeting(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/joinmeeting/");
        }
        public LiveMeetingScheduleDTO getsection(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getsection/");
        }
        public LiveMeetingScheduleDTO savedata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/savedata/");
        }
        public LiveMeetingScheduleDTO editdata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/editdata/");
        }
        public LiveMeetingScheduleDTO checkduplicate(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/checkduplicate/");
        }
        public LiveMeetingScheduleDTO getsubject(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getsubject/");
        }
      
        public LiveMeetingScheduleDTO deactive(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/deactive/");
        }

        //REPORT
        public LiveMeetingScheduleDTO getschrptdetailsprofile(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getschrptdetailsprofile/");
        }
        public LiveMeetingScheduleDTO getschrptdetails(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getschrptdetails/");
        }
        public LiveMeetingScheduleDTO getstudentprofiledata(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getstudentprofiledata/");
        }
        public LiveMeetingScheduleDTO getschedulereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getschedulereport/");
        }
        public LiveMeetingScheduleDTO getstaffprofilereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getstaffprofilereport/");
        }
        public LiveMeetingScheduleDTO getstudentprofilereport(LiveMeetingScheduleDTO data)
        {
            return COMMM.POSTPORTALData(data, "LiveMeetingScheduleFacade/getstudentprofilereport/");
        }
        
    }
}
