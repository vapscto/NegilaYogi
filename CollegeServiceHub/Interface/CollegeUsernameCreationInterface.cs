using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeUsernameCreationInterface
    {
        CollegeUsernameCreationDTO getalldetails(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO onyearchange(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO onCoursechange(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO onBranchchange(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO onSemchange(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO get_Studentdetails(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO saveatt(CollegeUsernameCreationDTO data);
        CollegeUsernameCreationDTO getStudentusername(CollegeUsernameCreationDTO data);
       Task<CollegeUsernameCreationDTO> SendSMS(CollegeUsernameCreationDTO data);
        
    }
}
