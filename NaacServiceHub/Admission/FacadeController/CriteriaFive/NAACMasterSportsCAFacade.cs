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
    public class NAACMasterSportsCAFacade : Controller
    {
        public NAACMasterSportsCAInterface _Interface;

        public NAACMasterSportsCAFacade(NAACMasterSportsCAInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACMasterSportsCADTO loaddata([FromBody] NAACMasterSportsCADTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACMasterSportsCADTO save([FromBody] NAACMasterSportsCADTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACMasterSportsCADTO deactiveStudent([FromBody] NAACMasterSportsCADTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACMasterSportsCADTO EditData([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.EditData(category);
        }
            [Route("viewuploadflies")]
        public NAACMasterSportsCADTO viewuploadflies([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.viewuploadflies(category);
        }
            [Route("deleteuploadfile")]
        public NAACMasterSportsCADTO deleteuploadfile([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.deleteuploadfile(category);
        }


        [Route("savemedicaldatawisecomments")]
        public NAACMasterSportsCADTO savemedicaldatawisecomments([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACMasterSportsCADTO getcomment([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACMasterSportsCADTO getfilecomment([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACMasterSportsCADTO savefilewisecomments([FromBody]NAACMasterSportsCADTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
