using System;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Staff;
using PreadmissionDTOs.com.vaps.College.Portals.Student;

namespace corewebapi18072016.Delegates.com.vapstech.College.Portals.Staff
{
    public class ClgStudentSearchDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgPortalAttendanceDTO, ClgPortalAttendanceDTO> COMMM = new CommonDelegate<ClgPortalAttendanceDTO, ClgPortalAttendanceDTO>();
        public ClgPortalAttendanceDTO getloaddata(ClgPortalAttendanceDTO data)
        {     
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getloaddata/");
        }
        public ClgPortalAttendanceDTO getcoursedata(ClgPortalAttendanceDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getcoursedata/");
        }
        
        public ClgPortalAttendanceDTO getbranchdata(ClgPortalAttendanceDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getbranchdata/");
        }
        public ClgPortalAttendanceDTO getsemdata(ClgPortalAttendanceDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getsemdata/");
        }
        public ClgPortalAttendanceDTO getstudent(ClgPortalAttendanceDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getstudent/");
        }        
        public ClgPortalAttendanceDTO getreport(ClgPortalAttendanceDTO data)
        {
            return COMMM.CLGPortalPOSTData(data, "ClgStudentSearchFacade/getreport/");
        }

        

    }
}
