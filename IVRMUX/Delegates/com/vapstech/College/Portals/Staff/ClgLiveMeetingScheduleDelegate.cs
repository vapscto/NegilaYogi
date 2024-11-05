using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff
{
    public class ClgLiveMeetingScheduleDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgLiveMeetingScheduleDTO, ClgLiveMeetingScheduleDTO> COMMM = new CommonDelegate<ClgLiveMeetingScheduleDTO, ClgLiveMeetingScheduleDTO>();
        public ClgLiveMeetingScheduleDTO getloaddata(ClgLiveMeetingScheduleDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getloaddata/");
        }
        public ClgLiveMeetingScheduleDTO getcoursedata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getcoursedata/");
        }
        
        public ClgLiveMeetingScheduleDTO getbranchdata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getbranchdata/");
        }
        public ClgLiveMeetingScheduleDTO getsemdata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getsemdata/");
        }
        public ClgLiveMeetingScheduleDTO getsection(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getsection/");
        }
        public ClgLiveMeetingScheduleDTO editdata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/editdata/");
        }        
        public ClgLiveMeetingScheduleDTO savedata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/savedata/");
        }
        public ClgLiveMeetingScheduleDTO deactive(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/deactive/");
        }
        //Staff PROFILE
        public ClgLiveMeetingScheduleDTO getempdetails(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getempdetails/");
        }
        public ClgLiveMeetingScheduleDTO ondatechange(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/ondatechange/");
        }
        public ClgLiveMeetingScheduleDTO onstartmeeting(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/onstartmeeting/");
        }
        public ClgLiveMeetingScheduleDTO endmainmeeting(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/endmainmeeting/");
        }
        public ClgLiveMeetingScheduleDTO joinmeeting(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/joinmeeting/");
        }

        //student

        public ClgLiveMeetingScheduleDTO getstudentdetails(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getstudentdetails/");
        }
        public ClgLiveMeetingScheduleDTO endmainmeetingstudent(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/endmainmeetingstudent/");
        }
        public ClgLiveMeetingScheduleDTO joinmeetingStudent(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/joinmeetingStudent/");
        }
        public ClgLiveMeetingScheduleDTO ondatechangestudent(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/ondatechangestudent/");
        }
          public ClgLiveMeetingScheduleDTO getschrptdetails(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getschrptdetails/");
        }
        public ClgLiveMeetingScheduleDTO getschrptdetailsprofile(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getschrptdetailsprofile/");
        }
          public ClgLiveMeetingScheduleDTO getschedulereport(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getschedulereport/");
        }

        public ClgLiveMeetingScheduleDTO getstaffprofilereport(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getstaffprofilereport/");
        }

        public ClgLiveMeetingScheduleDTO getstudentprofiledata(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getstudentprofiledata/");
        }
        public ClgLiveMeetingScheduleDTO getstudentprofilereport(ClgLiveMeetingScheduleDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgLiveMeetingScheduleFacade/getstudentprofilereport/");
        }

        

    }
}
