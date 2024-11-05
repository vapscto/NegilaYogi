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
    public class NAAC_MC_VACcommonFacade : Controller
    {

        public NAAC_MC_VACcommonInterface _Interface;
        public NAAC_MC_VACcommonFacade(NAAC_MC_VACcommonInterface Para)
        {
            _Interface = Para;
        }

      
        [Route("loaddata")]
        public NAAC_MC_VACcommon_DTO loaddata([FromBody] NAAC_MC_VACcommon_DTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("savedata141")]
        public NAAC_MC_VACcommon_DTO savedata141([FromBody] NAAC_MC_VACcommon_DTO data)
        {
            return _Interface.savedata141(data);
        }
        [Route("editdata141")]
        public NAAC_MC_VACcommon_DTO editdata141([FromBody]NAAC_MC_VACcommon_DTO data)
        {
            return _Interface.editdata141(data);
        }
        [Route("savedata142")]
        public NAAC_MC_VACcommon_DTO savedata142([FromBody] NAAC_MC_VACcommon_DTO data)
        {
            return _Interface.savedata142(data);
        }

        [Route("M_savedata221")]
        public NAAC_MC_VACcommon_DTO M_savedata221([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            return _Interface.M_savedata221(data);
        }

        [Route("M_savedata232")]
        public NAAC_MC_VACcommon_DTO M_savedata232([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            return _Interface.M_savedata232(data);
        }

        [Route("M_savedata254")]
        public NAAC_MC_VACcommon_DTO M_savedata254([FromBody]NAAC_MC_VACcommon_DTO data)
        {

            return _Interface.M_savedata254(data);
        }

    }
}
