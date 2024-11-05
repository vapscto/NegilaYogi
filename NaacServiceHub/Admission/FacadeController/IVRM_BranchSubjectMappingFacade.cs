using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class IVRM_BranchSubjectMappingFacade : Controller
    {
        public IVRM_BranchSubjectMappingInterface _Interface;

        public IVRM_BranchSubjectMappingFacade (IVRM_BranchSubjectMappingInterface para)
        {
            _Interface = para;
        }

        [Route("loaddata")]
        public IVRM_Master_Subjects_Branch_DTO loaddata([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.loaddata(data);
        }

        [Route("savedata")]
        public IVRM_Master_Subjects_Branch_DTO savedata([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.savedata(data);
        }

        [Route("editdata")]
        public IVRM_Master_Subjects_Branch_DTO editdata([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.editdata(data);
        }

        [Route("deactiveY")]
        public IVRM_Master_Subjects_Branch_DTO deactiveY([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.deactiveY(data);
        }

        [Route("get_Branch")]
        public IVRM_Master_Subjects_Branch_DTO get_Branch([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.get_Branch(data);
        }

        [Route("get_subject")]
        public IVRM_Master_Subjects_Branch_DTO get_subject([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            return _Interface.get_subject(data);
        }
        

    }
}
