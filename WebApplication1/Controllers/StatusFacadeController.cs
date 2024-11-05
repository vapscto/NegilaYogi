using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using WebApplication1.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class StatusFacadeController : Controller
    {
        public StatusInterface _IStatus;

        public StatusFacadeController(StatusInterface IStatus)
        {
            _IStatus = IStatus;
        }

        // load initial dropdown
        [Route("getinitialdata/{mi_id:int}")]
        public Task<CommonDTO> getInitialData(int mi_id)
        {
            return _IStatus.getInitailData(mi_id);
        }

        // get student on search filters
        [Route("getdataonsearchfilter")]
        public CommonDTO getdataonsearchfilter([FromBody] CommonDTO cdto)
        {
            return _IStatus.getdataonsearchfilter(cdto);
        }

        // save changed student status
        [Route("savedata")]
        public CommonDTO saveData([FromBody] CommonDTO cdto)
        {
            return  _IStatus.saveData(cdto); 
        }
    }
}
