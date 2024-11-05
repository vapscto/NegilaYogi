using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using WebApplication1.Services;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;


// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class MasterMenuPageMappingFacade : Controller
    {
        public MasterMenuPageMappingInterface _MasterModule;

        public MasterMenuPageMappingFacade(MasterMenuPageMappingInterface MasterpageModule)
        {
            _MasterModule = MasterpageModule;
        }
        
        [HttpGet]
        [Route("getalldetails/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO loaddata(int id)
        {
            return _MasterModule.loaddata(id);
        }

        [Route("modulechange/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO modchange(int id)
        {
            return _MasterModule.modchangedata(id);
        }

        [Route("mainmenuchange/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO mainmenuchan(int id)
        {
            return _MasterModule.mainmenuchangedata(id);
        }

        [Route("deletemasterdata/{id:int}")]
        public IVRM_Master_Menu_Page_MappingDTO deletemasterdata(int id)
        {
            return _MasterModule.deletemasterdataa(id);
        }

        [HttpPost]
        [Route("submenuchange")]
        public IVRM_Master_Menu_Page_MappingDTO submenuchan([FromBody] IVRM_Master_Menu_Page_MappingDTO data)
        {
            return _MasterModule.submenuchangedata(data);
        }

        [Route("savemenudata")]
        public IVRM_Master_Menu_Page_MappingDTO savedata([FromBody] IVRM_Master_Menu_Page_MappingDTO data)
        {
            return _MasterModule.savdata(data);
        }
    }
}
