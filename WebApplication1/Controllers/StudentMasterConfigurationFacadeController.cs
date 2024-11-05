using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class StudentMasterConfigurationFacadeController : Controller
    {
        // master configuration interface obj
        public StudentMasterConfigurationInterface _IstudentmasterConfig;

        public StudentMasterConfigurationFacadeController(StudentMasterConfigurationInterface Iobj)
        {
            _IstudentmasterConfig = Iobj;
        }

        [Route("getmasterdrp")]
        public Task<CommonDTO> getMasterConfigDrp([FromBody] CommonDTO data)
        {
            return _IstudentmasterConfig.getRecord(data);
        }

        [Route("getmastereditdata/{id:int}")]
        public MasterConfigurationDTO getMasterEditData(int id)
        {
            return _IstudentmasterConfig.getMasterEditData(id);
        }

     
        [Route("deletedetails")]
        public MasterConfigurationDTO deleteRecord([FromBody] MasterConfigurationDTO data)
        {
            return _IstudentmasterConfig.deleteRecord(data);
        }
        [HttpPost]
        public MasterConfigurationDTO saveMasterConfigData([FromBody] MasterConfigurationDTO mstConfigData)
        {
            return _IstudentmasterConfig.saveMasterConfig(mstConfigData);
        }
    }
}
