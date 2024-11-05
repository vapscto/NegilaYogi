using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeAttendanceEntryNewDelegate
    {

        CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO> _commbranch = new CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO>();

        public CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/getalldetails/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/getStudentdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getsubjectslist(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/getsubjectslist/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/getBatchdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO saveatt(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/saveatt/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeAttendanceEntryNewFacade/getsaveddatepreview/");
        }
    }
}
