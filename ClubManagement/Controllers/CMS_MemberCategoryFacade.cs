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
    public class CMS_MemberCategoryFacade : Controller
    {
        public CMS_MemberCategoryInterface _cms;
        public CMS_MemberCategoryFacade(CMS_MemberCategoryInterface cmsdept)
        {
            _cms = cmsdept;
        }
        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_MemberCategoryDTO loaddata(int id)
        {
            return _cms.loaddata(id);
            // return _cms.loaddata(id);
        }
        [HttpPost]
        [Route("savedata")]
        public CMS_MemberCategoryDTO savedata([FromBody]CMS_MemberCategoryDTO data)
        {
            return _cms.savedata(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MemberCategoryDTO deactive([FromBody]CMS_MemberCategoryDTO data)
        {
            return _cms.deactive(data);
        }
        [Route("edit")]
        public CMS_MemberCategoryDTO edit([FromBody]CMS_MemberCategoryDTO data)
        {
            return _cms.edit(data);
        }
    }
}
