using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgAttendanceEntryDelegate
    {
        CommonDelegate<ClgAttendanceEntryDTO, ClgAttendanceEntryDTO> _commbranch = new CommonDelegate<ClgAttendanceEntryDTO, ClgAttendanceEntryDTO>();

        public ClgAttendanceEntryDTO getalldetails(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getalldetails/");
        }
        //public ClgAttendanceEntryDTO getCoursedata(ClgAttendanceEntryDTO data)
        //{
        //    return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getCoursedata/");
        //}
        public ClgAttendanceEntryDTO getBranchdata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getBranchdata/");

        }
        public ClgAttendanceEntryDTO getSemesterdata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getSemesterdata/");
        }
        public ClgAttendanceEntryDTO getSectiondata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getSectiondata/");
        }
        public ClgAttendanceEntryDTO getSubjdata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getSubjdata/");
        }
        public ClgAttendanceEntryDTO getBatchdata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getBatchdata/");
        }
        public ClgAttendanceEntryDTO getStudentdata(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/getStudentdata/");
        }
        public ClgAttendanceEntryDTO saveatt(ClgAttendanceEntryDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgAttendanceEntryFacade/saveatt/");
        }        
    }
}
