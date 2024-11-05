using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Admission.Interface;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Admission.FacadeController
{
    [Route("api/[controller]")]
    public class NAACHrEducationFacade : Controller
    {
        public NAACHrEducationInterface _Interface;

        public NAACHrEducationFacade(NAACHrEducationInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACHrEducationDTO loaddata([FromBody] NAACHrEducationDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACHrEducationDTO save([FromBody] NAACHrEducationDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACHrEducationDTO deactiveStudent([FromBody] NAACHrEducationDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACHrEducationDTO EditData([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACHrEducationDTO viewuploadflies([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACHrEducationDTO deleteuploadfile([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("get_course")]
        public NAACHrEducationDTO get_course([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.get_course(category);
        }
         [Route("get_branch")]
        public NAACHrEducationDTO get_branch([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.get_branch(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACHrEducationDTO savemedicaldatawisecomments([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACHrEducationDTO getcomment([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACHrEducationDTO getfilecomment([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACHrEducationDTO savefilewisecomments([FromBody]NAACHrEducationDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
