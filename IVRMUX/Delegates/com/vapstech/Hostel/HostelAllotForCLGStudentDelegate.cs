using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelAllotForCLGStudentDelegate
    {
        CommonDelegate<HostelAllotForCLGStudentDTO, HostelAllotForCLGStudentDTO> _commnbranch = new CommonDelegate<HostelAllotForCLGStudentDTO, HostelAllotForCLGStudentDTO>();
        CommonDelegate<HL_Hostel_Student_Transfer_CollegeDTO, HL_Hostel_Student_Transfer_CollegeDTO> _commnbr = new CommonDelegate<HL_Hostel_Student_Transfer_CollegeDTO, HL_Hostel_Student_Transfer_CollegeDTO>();

        public HostelAllotForCLGStudentDTO loaddata(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/loaddata/");
        }
        public HostelAllotForCLGStudentDTO savedata(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/savedata/");
        }
        public HostelAllotForCLGStudentDTO get_studInfo(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_studInfo/");
        }
        public HostelAllotForCLGStudentDTO floor(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/floor/");
        }
        public HostelAllotForCLGStudentDTO room(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/room/");
        }
        public HostelAllotForCLGStudentDTO roomForVacateReport(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/roomForVacateReport/");
        }
        public HostelAllotForCLGStudentDTO roomdetails(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/roomdetails/");
        }
        public HostelAllotForCLGStudentDTO get_roomdetails(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_roomdetails/");
        }
        public HostelAllotForCLGStudentDTO editdata(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/editdata/");
        }

        public HostelAllotForCLGStudentDTO requestApproved(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/requestApproved/");
        }
        public HostelAllotForCLGStudentDTO requestRejected(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/requestRejected/");
        }
        public HL_Hostel_Student_Transfer_CollegeDTO HostelT(HL_Hostel_Student_Transfer_CollegeDTO data)
        {
            return _commnbr.Post_Hostel(data, "HostelAllotForCLGStudentFacade/HostelT/");
        }

        public HostelAllotForCLGStudentDTO get_course(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_course/");
        }

        public HostelAllotForCLGStudentDTO get_branch(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_branch/");
        }
        public HostelAllotForCLGStudentDTO get_sem(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_sem/");
        }
        //public HostelAllotForCLGStudentDTO get_sec(HostelAllotForCLGStudentDTO data)
        //{
        //    return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_sec/");
        //}
        public HostelAllotForCLGStudentDTO get_student(HostelAllotForCLGStudentDTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForCLGStudentFacade/get_student/");
        }
    }
}
