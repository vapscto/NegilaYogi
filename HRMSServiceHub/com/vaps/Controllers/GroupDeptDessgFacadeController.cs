using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.HRMS;
using HRMSServicesHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HRMSServicesHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class GroupDeptDessgFacadeController : Controller
    {
        // GET: api/values
        public GroupDeptDessgInterface _ads;

        public GroupDeptDessgFacadeController(GroupDeptDessgInterface adstu)
        {
            _ads = adstu;
        }

        // GET: api/values
        [Route("loaddata")]
        public HRGroupDeptDessgDTO getinitialdata([FromBody]HRGroupDeptDessgDTO dto)
        {
            return _ads.getBasicData(dto);
        }

        [Route("savedata")]
        public HRGroupDeptDessgDTO savedata([FromBody]HRGroupDeptDessgDTO dto)
        {
            return _ads.savedata(dto);
        }

        [Route("Editdata")]
        public HRGroupDeptDessgDTO Editdata([FromBody]HRGroupDeptDessgDTO dto)
        {
            return _ads.Editdata(dto);
        }
        [Route("masterDecative")]
        public HRGroupDeptDessgDTO masterDecative([FromBody]HRGroupDeptDessgDTO dto)
        {
            return _ads.masterDecative(dto);
        }
    }
}
