using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class IVRM_BranchSubjectMappingController : Controller
    {

        public IVRM_BranchSubjectMappingDelegate _Delobj = new IVRM_BranchSubjectMappingDelegate();

        [Route("loaddata/{id:int}")]
        public IVRM_Master_Subjects_Branch_DTO loaddata(int id)
        {
            IVRM_Master_Subjects_Branch_DTO data=new IVRM_Master_Subjects_Branch_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _Delobj.loaddata(data);
        }

        [Route("savedata")]
        public IVRM_Master_Subjects_Branch_DTO savedata([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
           
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _Delobj.savedata(data);
        }

        [Route("editdata")]
        public IVRM_Master_Subjects_Branch_DTO editdata([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _Delobj.editdata(data);
        }

        [Route("deactiveY")]
        public IVRM_Master_Subjects_Branch_DTO deactiveY([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));

            return _Delobj.deactiveY(data);
        }

        [Route("get_Branch")]
        public IVRM_Master_Subjects_Branch_DTO get_Branch([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _Delobj.get_Branch(data);
        }

        [Route("get_subject")]
        public IVRM_Master_Subjects_Branch_DTO get_subject([FromBody] IVRM_Master_Subjects_Branch_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _Delobj.get_subject(data);
        }

    }
}
