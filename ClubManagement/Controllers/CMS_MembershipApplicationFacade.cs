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
    public class CMS_MembershipApplicationFacade : Controller
    {
        public CMS_MembershipApplicationInterface _cms;
        public CMS_MembershipApplicationFacade(CMS_MembershipApplicationInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_MembershipApplicationDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_MembershipApplicationDTO savedata([FromBody]CMS_MembershipApplicationDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MembershipApplicationDTO deactive([FromBody]CMS_MembershipApplicationDTO data)
        {
            return _cms.deactive(data);
        }

    }
}
