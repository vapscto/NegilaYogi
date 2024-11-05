using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using PreadmissionDTOs;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class MasterMenuPageMappingController : Controller
    {
        MasterMenuPageMappingDelegate od = new MasterMenuPageMappingDelegate();
        [HttpGet]
        [Route("Getdetails/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO loaddata(int id)
        {
            return od.loaddata(id);
        }

        [Route("modulechange/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO modchange(int id)
        {
            return od.modchangedata(id);
        }

        [Route("mainmenuchange/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO mainmenuchan(int id)
        {
            return od.mainmenuchangedata(id);
        }

        [Route("deletemasterdata/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO deletemasterdata(int id)
        {
            return od.deletemasterdataa(id);
        }

        [HttpPost]
        [Route("submenuchange")]
        public IVRM_Master_Menu_Page_MappingDTO submenuchan([FromBody] IVRM_Master_Menu_Page_MappingDTO data)
        {
            return od.submenuchangedata(data);
        }

        [Route("savemenudata")]
        public IVRM_Master_Menu_Page_MappingDTO savedata([FromBody] IVRM_Master_Menu_Page_MappingDTO data)
        {
            return od.savdata(data);
        }

    }
}
