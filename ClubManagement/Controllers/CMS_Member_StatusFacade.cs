using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClubManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.ClubManagement;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClubManagement.Controllers
{
    [Route("api/[controller]")]
    public class CMS_Member_StatusFacade : Controller
    {
        public CMS_Member_StatusInterface _cms;

        public CMS_Member_StatusFacade(CMS_Member_StatusInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Member_StatusDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_Member_StatusDTO savedata([FromBody]CMS_Member_StatusDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_Member_StatusDTO deactive([FromBody]CMS_Member_StatusDTO data)
        {
            return _cms.deactive(data);
        }
    }
}
