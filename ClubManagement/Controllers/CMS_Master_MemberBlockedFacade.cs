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
    public class CMS_Master_MemberBlockedFacade : Controller
    {
        public CMS_Master_MemberBlockedInterface _cms;

        public CMS_Master_MemberBlockedFacade(CMS_Master_MemberBlockedInterface cmsdept)
        {
            _cms = cmsdept;
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_Master_MemberBlockedDTO loaddata(int id)
        {
            return _cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedetail1")]
        public CMS_Master_MemberBlockedDTO savedetail1([FromBody] CMS_Master_MemberBlockedDTO data)
        {
            return _cms.savedetail1(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_Master_MemberBlockedDTO deactive([FromBody] CMS_Master_MemberBlockedDTO data)
        {
            return _cms.deactive(data);
        }

    }
}
