using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeMultiHoursAttendanceEntryDelegate
    {
        CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO> _commbranch = new CommonDelegate<CollegeMultiHoursAttendanceEntryDTO, CollegeMultiHoursAttendanceEntryDTO>();

        public CollegeMultiHoursAttendanceEntryDTO getalldetails(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getalldetails/");
        }

        public CollegeMultiHoursAttendanceEntryDTO balgetalldetails(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/balgetalldetails/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getCoursedata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getCoursedata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getBranchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getBranchdata/");

        }
        public CollegeMultiHoursAttendanceEntryDTO getSemesterdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSemesterdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getSectiondata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSectiondata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getSubjdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getSubjdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getBatchdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getBatchdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getStudentdata(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getStudentdata/");
        }
        public CollegeMultiHoursAttendanceEntryDTO saveatt(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/saveatt/");
        }
        public CollegeMultiHoursAttendanceEntryDTO delete(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/delete/");
        }
        public CollegeMultiHoursAttendanceEntryDTO getsaveddatepreview(CollegeMultiHoursAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CollegeMultiHoursAttendanceEntryFacade/getsaveddatepreview/");
        }
        
    }
}
