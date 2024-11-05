using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.com.vapstech.Portals.IVRM;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.com.vapstech.Portals.IVRM
{
    [Route("api/[controller]")]
    public class IVRM_PrincipalMappingController : Controller
    {
        // GET: api/<controller>
        public IVRM_PrincipalMappingDelegate _objdel = new IVRM_PrincipalMappingDelegate();

        [Route("Getdetails/{id:int}")]
        public IVRM_PrincipalMappingDTO Getdetails(int id)
        {
            IVRM_PrincipalMappingDTO data = new IVRM_PrincipalMappingDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Getdetails(data);
        }

        [Route("saveclsdata")]
        public IVRM_PrincipalMappingDTO saveclsdata([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saveclsdata(data);
        }
        [Route("saveprncplstaf")]
        public IVRM_PrincipalMappingDTO saveprncplstaf([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.saveprncplstaf(data);
        }
        [Route("deactivehod")]
        public IVRM_PrincipalMappingDTO deactivehod([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.deactivehod(data);
        }

        [Route("Deactivatestaf")]
        public IVRM_PrincipalMappingDTO Deactivatestaf([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Deactivatestaf(data);
        }

        [Route("editprincipledata")]
        public IVRM_PrincipalMappingDTO editprincipledata([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.editprincipledata(data);
        }

        [Route("editprinciplestaffdata")]
        public IVRM_PrincipalMappingDTO editprinciplestaffdata([FromBody] IVRM_PrincipalMappingDTO data)
        {

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.editprinciplestaffdata(data);
        }

        [Route("onmodelclick")]
        public IVRM_PrincipalMappingDTO onmodelclick([FromBody] IVRM_PrincipalMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.onmodelclick(data);
        }

        [Route("Deactivateclass")]
        public IVRM_PrincipalMappingDTO Deactivateclass([FromBody] IVRM_PrincipalMappingDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _objdel.Deactivateclass(data);
        }


    }
}
