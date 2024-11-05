using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SportsServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ProgramMasterFacadeController : Controller
    {
        ProgramMasterInterface interobj;
        public ProgramMasterFacadeController(ProgramMasterInterface inter)
        {
            interobj = inter;
        }
        [Route("getDetails")]
        public ProgramMasterDTO getDetails([FromBody]ProgramMasterDTO data)
        {
            return interobj.getDetails(data);
        }
        [Route("save")]
        public ProgramMasterDTO save([FromBody]ProgramMasterDTO data)
        {
            return interobj.saveRecord(data);
        }
        [Route("EditDetails/{id:int}")]
        public ProgramMasterDTO EditDetails(int id)
        {
            return interobj.EditDetails(id);
        }
        [Route("deactivate")]
        public ProgramMasterDTO deactivateSponser([FromBody]ProgramMasterDTO data)
        {
            return interobj.deactivate(data);
        }

    }
}
