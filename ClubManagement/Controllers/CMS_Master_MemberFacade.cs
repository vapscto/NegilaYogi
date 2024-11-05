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
    public class CMS_Master_MemberFacade : Controller
    {
        public CMS_MasterMemberInerface _cms;

        public CMS_Master_MemberFacade(CMS_MasterMemberInerface cmsdept)
        {
            _cms = cmsdept;
        }

        [HttpGet]
        [Route("loaddata/{id:int}")]
        public CMS_MastermemberDTO loaddata(int id)
        {
            return _cms.loaddata(id);

        }
        [HttpPost]
        [Route("savedetail1")]
        public CMS_MastermemberDTO savedetail1([FromBody] CMS_MastermemberDTO data)
        {
            return _cms.savedetail1(data);
        }
        //editmember
        [Route("editmember")]
        public CMS_MastermemberDTO editmember([FromBody] CMS_MastermemberDTO data)
        {
            return _cms.editmember(data);
        }
        //savedetail2
        [Route("savedetail2")]
        public CMS_Master_Member_QualificationDTO savedetail2([FromBody] CMS_Master_Member_QualificationDTO data)
        {
            return _cms.savedetail2(data);
        }
        //savedetail3
        [Route("savedetail3")]
        public CMS_Master_Member_ExperienceDTO savedetail3([FromBody] CMS_Master_Member_ExperienceDTO data)
        {
            return _cms.savedetail3(data);
        }
        //savedetail5
        [Route("savedetail5")]
        public CMS_Master_MemberMobileNoDTO savedetail5([FromBody] CMS_Master_MemberMobileNoDTO data)
        {
            return _cms.savedetail5(data);
        }
        //savedetail6
        [Route("savedetail6")]
        public CMS_Master_Member_EmailDTO savedetail6([FromBody] CMS_Master_Member_EmailDTO data)
        {
            return _cms.savedetail6(data);
        }
        //savedetail7
        [Route("savedetail7")]
        public CMS_MasterMember_DocumentsDTO savedetail7([FromBody] CMS_MasterMember_DocumentsDTO data)
        {
            return _cms.savedetail7(data);
        }
        //deactive
        [Route("deactive")]
        public CMS_MastermemberDTO deactive([FromBody] CMS_MastermemberDTO data)
        {
            return _cms.deactive(data);
        }
    }
}
