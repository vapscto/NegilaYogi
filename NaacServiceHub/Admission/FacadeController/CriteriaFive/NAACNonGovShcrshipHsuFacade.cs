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
    public class NAACNonGovShcrshipHsuFacade : Controller
    {
        public NAACNonGovShcrshipHsuInterface _Interface;

        public NAACNonGovShcrshipHsuFacade(NAACNonGovShcrshipHsuInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACNonGovShcrshipHsuDTO loaddata([FromBody] NAACNonGovShcrshipHsuDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACNonGovShcrshipHsuDTO save([FromBody] NAACNonGovShcrshipHsuDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACNonGovShcrshipHsuDTO deactiveStudent([FromBody] NAACNonGovShcrshipHsuDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACNonGovShcrshipHsuDTO EditData([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.EditData(category);
        } [Route("viewuploadflies")]
        public NAACNonGovShcrshipHsuDTO viewuploadflies([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.viewuploadflies(category);
        }

        [Route("deleteuploadfile")]
        public NAACNonGovShcrshipHsuDTO deleteuploadfile([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACNonGovShcrshipHsuDTO savemedicaldatawisecomments([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACNonGovShcrshipHsuDTO getcomment([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACNonGovShcrshipHsuDTO getfilecomment([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACNonGovShcrshipHsuDTO savefilewisecomments([FromBody]NAACNonGovShcrshipHsuDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
