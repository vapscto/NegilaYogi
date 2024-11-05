using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class IVRM_BranchSubjectMappingDelegate
    {
        CommonDelegate<IVRM_Master_Subjects_Branch_DTO, IVRM_Master_Subjects_Branch_DTO> _COMM = new CommonDelegate<IVRM_Master_Subjects_Branch_DTO, IVRM_Master_Subjects_Branch_DTO>();

        public IVRM_Master_Subjects_Branch_DTO loaddata(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/loaddata");
        }

        public IVRM_Master_Subjects_Branch_DTO savedata(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/savedata");
        }

        public IVRM_Master_Subjects_Branch_DTO editdata(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/editdata");
        }

        public IVRM_Master_Subjects_Branch_DTO deactiveY(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/deactiveY");
        }

        public IVRM_Master_Subjects_Branch_DTO get_Branch(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/get_Branch");
        }

        public IVRM_Master_Subjects_Branch_DTO get_subject(IVRM_Master_Subjects_Branch_DTO data)
        {
            return _COMM.naacdetailsbypost(data, "IVRM_BranchSubjectMappingFacade/get_subject");
        }


    }
}
