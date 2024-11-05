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
    public class NAACEncDevSchemeFacade : Controller
    {
        public NAACEncDevSchemeInterface _Interface;

        public NAACEncDevSchemeFacade(NAACEncDevSchemeInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACEncDevSchemeDTO loaddata([FromBody] NAACEncDevSchemeDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACEncDevSchemeDTO save([FromBody] NAACEncDevSchemeDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACEncDevSchemeDTO deactiveStudent([FromBody] NAACEncDevSchemeDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACEncDevSchemeDTO EditData([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.EditData(category);
        }
            [Route("viewuploadflies")]
        public NAACEncDevSchemeDTO viewuploadflies([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
            [Route("deleteuploadfile")]
        public NAACEncDevSchemeDTO deleteuploadfile([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACEncDevSchemeDTO savemedicaldatawisecomments([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACEncDevSchemeDTO getcomment([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACEncDevSchemeDTO getfilecomment([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACEncDevSchemeDTO savefilewisecomments([FromBody]NAACEncDevSchemeDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
