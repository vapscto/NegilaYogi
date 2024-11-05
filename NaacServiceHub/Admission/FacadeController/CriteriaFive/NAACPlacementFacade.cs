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
    public class NAACPlacementFacade : Controller
    {
        public NAACPlacementInterface _Interface;

        public NAACPlacementFacade(NAACPlacementInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACPlacementDTO loaddata([FromBody] NAACPlacementDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACPlacementDTO save([FromBody] NAACPlacementDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACPlacementDTO deactiveStudent([FromBody] NAACPlacementDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACPlacementDTO EditData([FromBody]NAACPlacementDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACPlacementDTO viewuploadflies([FromBody]NAACPlacementDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACPlacementDTO deleteuploadfile([FromBody]NAACPlacementDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("get_course")]
        public NAACPlacementDTO get_course([FromBody]NAACPlacementDTO category)
        {
            return _Interface.get_course(category);
        }
         [Route("get_branch")]
        public NAACPlacementDTO get_branch([FromBody]NAACPlacementDTO category)
        {
            return _Interface.get_branch(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACPlacementDTO savemedicaldatawisecomments([FromBody]NAACPlacementDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACPlacementDTO getcomment([FromBody]NAACPlacementDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACPlacementDTO getfilecomment([FromBody]NAACPlacementDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACPlacementDTO savefilewisecomments([FromBody]NAACPlacementDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
