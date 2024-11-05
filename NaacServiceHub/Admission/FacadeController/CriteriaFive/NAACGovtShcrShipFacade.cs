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
    public class NAACGovtShcrShipFacade : Controller
    {
        public NAACGovtShcrShipInterface _Interface;

        public NAACGovtShcrShipFacade(NAACGovtShcrShipInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACGovtShcrShipDTO loaddata([FromBody] NAACGovtShcrShipDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACGovtShcrShipDTO save([FromBody] NAACGovtShcrShipDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACGovtShcrShipDTO deactiveStudent([FromBody] NAACGovtShcrShipDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACGovtShcrShipDTO EditData([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.EditData(category);
        }
        [Route("viewuploadflies")]
        public NAACGovtShcrShipDTO viewuploadflies([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("deleteuploadfile")]
        public NAACGovtShcrShipDTO deleteuploadfile([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACGovtShcrShipDTO savemedicaldatawisecomments([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACGovtShcrShipDTO getcomment([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACGovtShcrShipDTO getfilecomment([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACGovtShcrShipDTO savefilewisecomments([FromBody]NAACGovtShcrShipDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }

    }
}
