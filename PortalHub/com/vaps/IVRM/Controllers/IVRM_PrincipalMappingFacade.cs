using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRM.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Principal;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_PrincipalMappingFacade : Controller
    {
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public IVRM_PrincipalMappingInterface _objInter;

        public IVRM_PrincipalMappingFacade(IVRM_PrincipalMappingInterface para)
        {
            _objInter = para;
        }

        [Route("Getdetails")]
        public Task<IVRM_PrincipalMappingDTO> Getdetails([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.GetdetailsAsync(data);
        }


        [Route("saveclsdata")]
        public IVRM_PrincipalMappingDTO saveclsdata([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.saveclsdata(data);
        }

        [Route("saveprncplstaf")]
        public IVRM_PrincipalMappingDTO saveprncplstaf([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.saveprncplstaf(data);
        }

        [Route("deactivehod")]
        public IVRM_PrincipalMappingDTO deactivehod([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.deactivehod(data);
        }

        [Route("Deactivatestaf")]
        public IVRM_PrincipalMappingDTO Deactivatestaf([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.Deactivatestaf(data);
        }

        [Route("editprincipledata")]
        public IVRM_PrincipalMappingDTO editprincipledata([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.editprincipledata(data);
        }

        [Route("editprinciplestaffdata")]
        public IVRM_PrincipalMappingDTO editprinciplestaffdata([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.editprinciplestaffdata(data);
        }

        [Route("onmodelclick")]
        public IVRM_PrincipalMappingDTO onmodelclick([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.onmodelclick(data);
        }

        [Route("Deactivateclass")]
        public IVRM_PrincipalMappingDTO Deactivateclass([FromBody] IVRM_PrincipalMappingDTO data)
        {
            return _objInter.Deactivateclass(data);
        }

    }
}
