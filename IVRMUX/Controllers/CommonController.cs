using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        CommonDelegate _commonDelegate = new CommonDelegate();
        private static FacadeUrl _config;
        private FacadeUrl fdu = new FacadeUrl();

        public CommonController(IOptions<FacadeUrl> settings)
        {
            _config = settings.Value;
            new CommonDelegate(_config);
            fdu = _config;
        }

        [Route("getPreviledgs/{id:int}")]
        public CommonDTO getPagePreviledgs(int id)
        {
            int roleId = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            CommonDTO ctdo = new CommonDTO();
            ctdo.pageId = id;
            ctdo.roleId = 12;

            return _commonDelegate.getPagePreviledgs(ctdo);
        }
    }
}