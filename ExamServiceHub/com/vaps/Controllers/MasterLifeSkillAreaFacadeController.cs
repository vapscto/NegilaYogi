
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamServiceHub.com.vaps.Interfaces;
//using AdmissionServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.Exam;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ExamServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterLifeSkillAreaFacadeController : Controller
    {
        public MasterLifeSkillAreaInterface _MasterLifeSkillArea;

        public MasterLifeSkillAreaFacadeController(MasterLifeSkillAreaInterface MasterLifeSkillArea)
        {
            _MasterLifeSkillArea = MasterLifeSkillArea;
        }


        [Route("Getdetails")]
        public MasterLifeSkillAreaDTO Getdetails([FromBody]MasterLifeSkillAreaDTO data)//int IVRMM_Id
        {
           
            return _MasterLifeSkillArea.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public MasterLifeSkillAreaDTO editdetails(int ID)
        {
            return _MasterLifeSkillArea.editdetails(ID);
        }
        

        [Route("savedata")]
        public MasterLifeSkillAreaDTO savedata([FromBody] MasterLifeSkillAreaDTO data)
        {
            return _MasterLifeSkillArea.savedata(data);
        }
       
        [Route("deactivate")]
        public MasterLifeSkillAreaDTO deactivate([FromBody] MasterLifeSkillAreaDTO data)
        {           
            return _MasterLifeSkillArea.deactivate(data);
        }
        [Route("validateordernumber")]
        public MasterLifeSkillAreaDTO validateordernumber([FromBody] MasterLifeSkillAreaDTO data)
        {
            return _MasterLifeSkillArea.validateordernumber(data);
        }

    }
}
