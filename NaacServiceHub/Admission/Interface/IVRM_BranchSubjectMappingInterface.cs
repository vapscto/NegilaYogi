using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface IVRM_BranchSubjectMappingInterface
    {

        IVRM_Master_Subjects_Branch_DTO loaddata(IVRM_Master_Subjects_Branch_DTO data);
        IVRM_Master_Subjects_Branch_DTO savedata(IVRM_Master_Subjects_Branch_DTO data);
        IVRM_Master_Subjects_Branch_DTO editdata(IVRM_Master_Subjects_Branch_DTO data);
        IVRM_Master_Subjects_Branch_DTO deactiveY(IVRM_Master_Subjects_Branch_DTO data);
        IVRM_Master_Subjects_Branch_DTO get_Branch(IVRM_Master_Subjects_Branch_DTO data);
        IVRM_Master_Subjects_Branch_DTO get_subject(IVRM_Master_Subjects_Branch_DTO data);

    }
}
