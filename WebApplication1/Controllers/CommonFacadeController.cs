using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class CommonFacadeController : Controller
    {
        public CommonInterface _Icommon;

        public CommonFacadeController(CommonInterface icomm)
        {
            _Icommon = icomm;
        }

        [Route("getpagepreviledges")]
        public Task<CommonDTO> getPagePreviledges([FromBody] CommonDTO cdto)
        {
            return _Icommon.getPagePreviledges(cdto);
        }
    }
}
