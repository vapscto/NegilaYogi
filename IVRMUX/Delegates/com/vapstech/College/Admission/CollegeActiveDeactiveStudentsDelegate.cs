using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeActiveDeactiveStudentsDelegate
    {
        CommonDelegate<CollegeActiveDeactiveStudentsDTO, CollegeActiveDeactiveStudentsDTO> _comm = new CommonDelegate<CollegeActiveDeactiveStudentsDTO, CollegeActiveDeactiveStudentsDTO>();

        public CollegeActiveDeactiveStudentsDTO getdata(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/getdata/");
        }
        public CollegeActiveDeactiveStudentsDTO onacademicyearchange(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/onacademicyearchange/");
        }
        public CollegeActiveDeactiveStudentsDTO oncoursechange(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/oncoursechange/");
        }
        public CollegeActiveDeactiveStudentsDTO onbranchchange(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/onbranchchange/");
        }
        public CollegeActiveDeactiveStudentsDTO onchangesemester(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/onchangesemester/");
        }
        public CollegeActiveDeactiveStudentsDTO search(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/search/");
        }
        public CollegeActiveDeactiveStudentsDTO savedata(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/savedata/");
        }
        public CollegeActiveDeactiveStudentsDTO getreport(CollegeActiveDeactiveStudentsDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeActiveDeactiveStudentsFacade/getreport/");
        }
        
    }
}
