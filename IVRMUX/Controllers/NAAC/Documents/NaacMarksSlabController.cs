using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Documents;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Documents
{
    [Route("api/[controller]")]
    public class NaacMarksSlabController : Controller
    {
        public NaacMarksSlabDelegate _Delegate = new NaacMarksSlabDelegate();

        [Route("Getdetails/{id:int}")]
        public NAAC_AC_Criteria_MarksSlab_DTO Getdetails(int id)
        {
            NAAC_AC_Criteria_MarksSlab_DTO data = new NAAC_AC_Criteria_MarksSlab_DTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            return _Delegate.Getdetails(data);
        }

        [Route("savedata")]
        public NAAC_AC_Criteria_MarksSlab_DTO savedata([FromBody]NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));           
            return _Delegate.savedata(data);
        }
        [Route("editdata")]
        public NAAC_AC_Criteria_MarksSlab_DTO editdata([FromBody]NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delegate.editdata(data);
        }
        [Route("deactive")]
        public NAAC_AC_Criteria_MarksSlab_DTO deactive([FromBody]NAAC_AC_Criteria_MarksSlab_DTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _Delegate.deactive(data);
        }
        
    }
}
