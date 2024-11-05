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
    public class NAACCompExamsFacade : Controller
    {
        public NAACCompExamsInterface _Interface;

        public NAACCompExamsFacade(NAACCompExamsInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACCompExamsDTO loaddata([FromBody] NAACCompExamsDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACCompExamsDTO save([FromBody] NAACCompExamsDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACCompExamsDTO deactiveStudent([FromBody] NAACCompExamsDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACCompExamsDTO EditData([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.EditData(category);
        }
                [Route("deleteuploadfile")]
        public NAACCompExamsDTO deleteuploadfile([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
                [Route("viewuploadflies")]
        public NAACCompExamsDTO viewuploadflies([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACCompExamsDTO savemedicaldatawisecomments([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACCompExamsDTO getcomment([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACCompExamsDTO getfilecomment([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACCompExamsDTO savefilewisecomments([FromBody]NAACCompExamsDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }

    }
}
