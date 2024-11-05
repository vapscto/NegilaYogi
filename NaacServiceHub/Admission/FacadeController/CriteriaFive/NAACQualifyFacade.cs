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
    public class NAACQualifyFacade : Controller
    {
        public NAACQualifyInterface _Interface;

        public NAACQualifyFacade(NAACQualifyInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACQualifyDTO loaddata([FromBody] NAACQualifyDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save1")]
        public NAACQualifyDTO save1([FromBody] NAACQualifyDTO data)
        {
            return _Interface.save1(data);
        }

        [Route("deactiveStudent1")]
        public NAACQualifyDTO deactiveStudent1([FromBody] NAACQualifyDTO data)
        {
            return _Interface.deactiveStudent1(data);
        }

        [Route("EditData1")]
        public NAACQualifyDTO EditData1([FromBody]NAACQualifyDTO category)
        {
            return _Interface.EditData1(category);
        }

        [Route("save")]
        public NAACQualifyDTO save([FromBody] NAACQualifyDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACQualifyDTO deactiveStudent([FromBody] NAACQualifyDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACQualifyDTO EditData([FromBody]NAACQualifyDTO category)
        {
            return _Interface.EditData(category);
        }


        [Route("viewuploadflies")]
        public NAACQualifyDTO viewuploadflies([FromBody]NAACQualifyDTO category)
        {
            return _Interface.viewuploadflies(category);
        }

        [Route("deleteuploadfile")]
        public NAACQualifyDTO deleteuploadfile([FromBody]NAACQualifyDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACQualifyDTO savemedicaldatawisecomments([FromBody]NAACQualifyDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACQualifyDTO getcomment([FromBody]NAACQualifyDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACQualifyDTO getfilecomment([FromBody]NAACQualifyDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACQualifyDTO savefilewisecomments([FromBody]NAACQualifyDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
