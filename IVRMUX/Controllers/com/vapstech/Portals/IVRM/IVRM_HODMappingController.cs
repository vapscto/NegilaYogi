using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.IVRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRM
{
    [Route("api/[controller]")]
    public class IVRM_HODMappingController : Controller
    {
        // GET: api/<controller>
        public IVRM_HODMappingDelegate _objdel = new IVRM_HODMappingDelegate();

        [Route("Getdetails/{id:int}")]
        public IVRM_HodMappingDTO Getdetails(int id)
        {
            IVRM_HodMappingDTO data = new IVRM_HodMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Getdetails(data); 
        }
        
        [Route("saveclsdata")]
        public IVRM_HodMappingDTO saveclsdata([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saveclsdata(data);
        }
        [Route("savehodstaf")]
        public IVRM_HodMappingDTO savehodstaf([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.savehodstaf(data);
        }
        [Route("deactivehod")]
        public IVRM_HodMappingDTO deactivehod([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.deactivehod(data);
        }

        [Route("Deactivatestaf")]
        public IVRM_HodMappingDTO Deactivatestaf([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Deactivatestaf(data);
        }

        [Route("editHoddata")]
        public IVRM_HodMappingDTO editHoddata([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.editHoddata(data);
        }

        [Route("editHodStaffdata")]
        public IVRM_HodMappingDTO editHodStaffdata([FromBody] IVRM_HodMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.editHodStaffdata(data);
        }   

        [Route("onmodelclick")]
        public IVRM_HodMappingDTO onmodelclick([FromBody] IVRM_HodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.onmodelclick(data);
        }

        [Route("Deactivateclass")]
        public IVRM_HodMappingDTO Deactivateclass([FromBody] IVRM_HodMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Deactivateclass(data);
        }
        
    }
}
