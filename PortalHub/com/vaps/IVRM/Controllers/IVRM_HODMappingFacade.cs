using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortalHub.com.vaps.IVRM.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.HOD;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalHub.com.vaps.IVRM.Controllers
{
    [Route("api/[controller]")]
    public class IVRM_HODMappingFacade : Controller
    {
        // GET: api/<controller>

        public IVRM_HODMappingInterface _objInter;

        public IVRM_HODMappingFacade(IVRM_HODMappingInterface para)
        {
            _objInter = para;
        }

        [Route("Getdetails")]
        public Task<IVRM_HodMappingDTO> Getdetails([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.GetdetailsAsync(data);
        }

       
        [Route("saveclsdata")]
        public IVRM_HodMappingDTO saveclsdata([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.saveclsdata(data);
        }

        [Route("savehodstaf")]
        public IVRM_HodMappingDTO savehodstaf([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.savehodstaf(data);
        }

        [Route("deactivehod")]
        public IVRM_HodMappingDTO deactivehod([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.deactivehod(data);
        }

        [Route("Deactivatestaf")]
        public IVRM_HodMappingDTO Deactivatestaf([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.Deactivatestaf(data);
        }

        [Route("editHoddata")]
        public IVRM_HodMappingDTO editHoddata([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.editHoddata(data);
        }

        [Route("editHodStaffdata")]
        public IVRM_HodMappingDTO editHodStaffdata([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.editHodStaffdata(data);
        }

        [Route("onmodelclick")]
        public IVRM_HodMappingDTO onmodelclick([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.onmodelclick(data);
        }

        [Route("Deactivateclass")]
        public IVRM_HodMappingDTO Deactivateclass([FromBody] IVRM_HodMappingDTO data)
        {
            return _objInter.Deactivateclass(data);
        }

        
    }
}
