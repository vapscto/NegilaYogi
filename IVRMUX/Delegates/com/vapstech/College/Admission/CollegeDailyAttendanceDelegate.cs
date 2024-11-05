using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeDailyAttendanceDelegate
    {
        CommonDelegate<CollegeDailyAttendanceDTO, CollegeDailyAttendanceDTO> _commbranch = new CommonDelegate<CollegeDailyAttendanceDTO, CollegeDailyAttendanceDTO>();
        public CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/getdetails/");
        }
        public CollegeDailyAttendanceDTO onselectAcdYear(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onselectAcdYear/");
        }
        public CollegeDailyAttendanceDTO onselectCourse(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onselectCourse/");
        }
        public CollegeDailyAttendanceDTO onselectBranch(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onselectBranch/");
        }
        public CollegeDailyAttendanceDTO getsection(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/getsection/");
        }
        public CollegeDailyAttendanceDTO getsubject(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/getsubject/");
        }



        public CollegeDailyAttendanceDTO onreport(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onreport/");
        }
        public CollegeDailyAttendanceDTO onreportpercentage(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onreportpercentage/");
        }
        public CollegeDailyAttendanceDTO getAttendetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/getAttendetails/");
        }
        public CollegeDailyAttendanceDTO GetAttendancedetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/GetAttendancedetails/");
        }
        public CollegeDailyAttendanceDTO getStudentAbsentDetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/getStudentAbsentDetails/");
        }
        //GetAttendancedetails
        public CollegeDailyAttendanceDTO absentsendsms(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/absentsendsms/");
        }
        public CollegeDailyAttendanceDTO onreportshortagepercentage(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onreportshortagepercentage/");
        }
        public CollegeDailyAttendanceDTO onreporttotalattendance(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeDailyAttendanceFacade/onreporttotalattendance/");
        }
        
    }
}
