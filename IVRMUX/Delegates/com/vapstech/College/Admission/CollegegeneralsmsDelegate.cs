using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegegeneralsmsDelegate
    {
        CommonDelegate<CollegegeneralsmsDTO, CollegegeneralsmsDTO> comm = new CommonDelegate<CollegegeneralsmsDTO, CollegegeneralsmsDTO>();

        public CollegegeneralsmsDTO Getdetails(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/Getdetails/");
        }

        public CollegegeneralsmsDTO Getexam(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/Getexam/");
        }
        public CollegegeneralsmsDTO GetEmployeeDetailsByLeaveYearAndMonth(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/GetEmployeeDetailsByLeaveYearAndMonth/");
        }
        public CollegegeneralsmsDTO Getdepartment(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/Getdepartment/");
        }
        public CollegegeneralsmsDTO get_designation(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/get_designation/");
        }
        public CollegegeneralsmsDTO get_employee(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/get_employee/");
        }
        public CollegegeneralsmsDTO savedetail(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/savedetail/");
        }
        public CollegegeneralsmsDTO GetClass(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/GetClass/");
        }
        public CollegegeneralsmsDTO GetSection(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/GetSection/");
        }
        public CollegegeneralsmsDTO createuser(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/createuser/");
        }
        public CollegegeneralsmsDTO GetStudentDetails(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/GetStudentDetails/");
        }
        public CollegegeneralsmsDTO onSelectyear(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/onSelectyear/");
        }
        public CollegegeneralsmsDTO onselectedcourse(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/onselectedcourse/");
        }
        public CollegegeneralsmsDTO onselectbranch(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/onselectbranch/");
        }
        public CollegegeneralsmsDTO onselectsemister(CollegegeneralsmsDTO data)
        {
            return comm.clgadmissionbypost(data, "CollegegeneralsmsFacade/onselectsemister/");
        }

    }
}
