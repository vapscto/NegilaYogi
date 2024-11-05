using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class CollegeUsernameCreationDelegate
    {
        CommonDelegate<CollegeUsernameCreationDTO, CollegeUsernameCreationDTO> _comm = new CommonDelegate<CollegeUsernameCreationDTO, CollegeUsernameCreationDTO>();
        public CollegeUsernameCreationDTO getalldetails(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/getalldetails");
        }
        public CollegeUsernameCreationDTO onyearchange(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/onyearchange");
        }
        public CollegeUsernameCreationDTO onCoursechange(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/onCoursechange");
        }
        public CollegeUsernameCreationDTO onBranchchange(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/onBranchchange");
        }
        public CollegeUsernameCreationDTO onSemchange(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/onSemchange");
        }
        public CollegeUsernameCreationDTO get_Studentdetails(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/get_Studentdetails");
        }
        public CollegeUsernameCreationDTO saveatt(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/saveatt");
        }
        public CollegeUsernameCreationDTO getStudentusername(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/getStudentusername");
        }
        public CollegeUsernameCreationDTO SendSMS(CollegeUsernameCreationDTO data)
        {
            return _comm.clgadmissionbypost(data, "CollegeUsernameCreationFacade/SendSMS");
        }
        
    }
}
