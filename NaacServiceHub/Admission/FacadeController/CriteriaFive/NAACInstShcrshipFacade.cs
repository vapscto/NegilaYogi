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
    public class NAACInstShcrshipFacade : Controller
    {
        public NAACInstShcrshipInterface _Interface;

        public NAACInstShcrshipFacade(NAACInstShcrshipInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACInstShcrshipDTO loaddata([FromBody] NAACInstShcrshipDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACInstShcrshipDTO save([FromBody] NAACInstShcrshipDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACInstShcrshipDTO deactiveStudent([FromBody] NAACInstShcrshipDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACInstShcrshipDTO EditData([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.EditData(category);
        } [Route("viewuploadflies")]
        public NAACInstShcrshipDTO viewuploadflies([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.viewuploadflies(category);
        }

        [Route("deleteuploadfile")]
        public NAACInstShcrshipDTO deleteuploadfile([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACInstShcrshipDTO savemedicaldatawisecomments([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACInstShcrshipDTO getcomment([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACInstShcrshipDTO getfilecomment([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACInstShcrshipDTO savefilewisecomments([FromBody]NAACInstShcrshipDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
