using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using NaacServiceHub.Admission.Interface.Criteria7;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController.Criteria7
{
    [Route("api/[controller]")]
    public class AuditOnEnvironmentFacade : Controller
    {
        public AuditOnEnvironmentInterface _Iobj;
        public AuditOnEnvironmentFacade(AuditOnEnvironmentInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_MC_716_AuditOnEnvironment_DTO> loaddata([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.loaddata(data);
        }

        [Route("savedatatab1")]
        public NAAC_MC_716_AuditOnEnvironment_DTO savedatatab1([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.savedatatab1(data);
        }

        [Route("editTab1")]
        public NAAC_MC_716_AuditOnEnvironment_DTO editTab1([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.editTab1(data);
        }

        [Route("deactivYTab1")]
        public NAAC_MC_716_AuditOnEnvironment_DTO deactivYTab1([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.deactivYTab1(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_MC_716_AuditOnEnvironment_DTO deleteuploadfile([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }

        [Route("getData")]
        public NAAC_MC_716_AuditOnEnvironment_DTO getData([FromBody] NAAC_MC_716_AuditOnEnvironment_DTO data)
        {
            return _Iobj.getData(data);
        }
    }
}
