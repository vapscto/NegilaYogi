using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CollegeServiceHub.Interface
{
    public interface CollegeAdmssionCancelProcessInterface
    {
        CollegeAdmssionCancelProcessDTO getalldetails(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO onyearchange(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO onCoursechange(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO onBranchchange(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO onSemchange(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO get_Studentdetails(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO saveatt(CollegeAdmssionCancelProcessDTO data);
        CollegeAdmssionCancelProcessDTO getStudentdetails(CollegeAdmssionCancelProcessDTO data);
        
    }
}
