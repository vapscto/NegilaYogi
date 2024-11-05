using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeAdmssionCancelProcessDelegate
    {
        CommonDelegate<CollegeAdmssionCancelProcessDTO, CollegeAdmssionCancelProcessDTO> _comm = new CommonDelegate<CollegeAdmssionCancelProcessDTO, CollegeAdmssionCancelProcessDTO>();
        public CollegeAdmssionCancelProcessDTO getalldetails(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/getalldetails");
        }
        public CollegeAdmssionCancelProcessDTO onyearchange(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/onyearchange");
        }
        public CollegeAdmssionCancelProcessDTO onCoursechange(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/onCoursechange");
        }
        public CollegeAdmssionCancelProcessDTO onBranchchange(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/onBranchchange");
        }
        public CollegeAdmssionCancelProcessDTO onSemchange(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/onSemchange");
        }
        public CollegeAdmssionCancelProcessDTO get_Studentdetails(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/get_Studentdetails");
        }
        public CollegeAdmssionCancelProcessDTO saveatt(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/saveatt");
        }
        public CollegeAdmssionCancelProcessDTO getStudentdetails(CollegeAdmssionCancelProcessDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeAdmssionCancelProcessFacade/getStudentdetails");
        }
        
    }
}
