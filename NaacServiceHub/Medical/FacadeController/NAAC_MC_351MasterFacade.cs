using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class NAAC_MC_351MasterFacade : Controller
    {
        public NAAC_MC_351MasterInterface _Interface;

        public NAAC_MC_351MasterFacade(NAAC_MC_351MasterInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public NAAC_MC_351_CollaborationActivities_DTO loaddata([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {

            return _Interface.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_MC_351_CollaborationActivities_DTO savedata([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {


            return _Interface.savedata(data);
        }

        [Route("editdata")]
        public NAAC_MC_351_CollaborationActivities_DTO editdata([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {


            return _Interface.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_MC_351_CollaborationActivities_DTO deactivY([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {


            return _Interface.deactivY(data);
        }

        [Route("viewuploadflies")]
        public NAAC_MC_351_CollaborationActivities_DTO viewuploadflies([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_MC_351_CollaborationActivities_DTO deleteuploadfile([FromBody] NAAC_MC_351_CollaborationActivities_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }
    }
}
