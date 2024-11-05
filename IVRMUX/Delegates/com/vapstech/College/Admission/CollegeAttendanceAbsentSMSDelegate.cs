using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeAttendanceAbsentSMSDelegate
    {

        CommonDelegate<CollegeDailyAttendanceDTO, CollegeDailyAttendanceDTO> _commbranch = new CommonDelegate<CollegeDailyAttendanceDTO, CollegeDailyAttendanceDTO>();

        public CollegeDailyAttendanceDTO getdetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/getdetails/");
        }
        public CollegeDailyAttendanceDTO onselectAcdYear(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/onselectAcdYear/");
        }
        public CollegeDailyAttendanceDTO onselectCourse(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/onselectCourse/");
        }
        public CollegeDailyAttendanceDTO onselectBranch(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/onselectBranch/");
        }
        public CollegeDailyAttendanceDTO getsection(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/getsection/");
        }
        public CollegeDailyAttendanceDTO getAttendetails(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/getAttendetails/");
        }
        public CollegeDailyAttendanceDTO absentsendsms(CollegeDailyAttendanceDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CollegeAttendanceAbsentSMSFacade/absentsendsms/");
        }

    }
}
