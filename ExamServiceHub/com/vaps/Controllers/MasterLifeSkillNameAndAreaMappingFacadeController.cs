
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
    public class MasterLifeSkillNameAndAreaMappingFacadeController : Controller
    {
        public MasterLifeSkillAreaMappingInterface _MasterLifeSkillArea;

        public MasterLifeSkillNameAndAreaMappingFacadeController(MasterLifeSkillAreaMappingInterface MasterLifeSkillArea)
        {
            _MasterLifeSkillArea = MasterLifeSkillArea;
        }


        [Route("Getdetails")]
        public MasterLifeSkillAreaMappingDTO Getdetails([FromBody]MasterLifeSkillAreaMappingDTO data)//int IVRMM_Id
        {
           
            return _MasterLifeSkillArea.Getdetails(data);
           
        }

        [Route("editdetails/{id:int}")]
        public MasterLifeSkillAreaMappingDTO editdetails(int ID)
        {
            return _MasterLifeSkillArea.editdetails(ID);
        }


        [Route("getgrade")]
        public MasterLifeSkillAreaMappingDTO getgrade([FromBody] MasterLifeSkillAreaMappingDTO data)
        {
            return _MasterLifeSkillArea.getgrade(data);
        }




        [Route("savedata")]
        public MasterLifeSkillAreaMappingDTO savedata([FromBody] MasterLifeSkillAreaMappingDTO data)
        {
            return _MasterLifeSkillArea.savedata(data);
        }
       
        [Route("deactivate")]
        public MasterLifeSkillAreaMappingDTO deactivate([FromBody] MasterLifeSkillAreaMappingDTO data)
        {           
            return _MasterLifeSkillArea.deactivate(data);
        }     
       

    }
}
