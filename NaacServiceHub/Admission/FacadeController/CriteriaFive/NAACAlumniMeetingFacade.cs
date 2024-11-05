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
    public class NAACAlumniMeetingFacade: Controller
    {
        public NAACAlumniMeetingInterface _Interface;

        public NAACAlumniMeetingFacade(NAACAlumniMeetingInterface q)
        {
            _Interface = q;
        }

        [Route("loaddata")]
        public NAACAlumniMeetingDTO loaddata([FromBody] NAACAlumniMeetingDTO data)
        {
            return _Interface.loaddata(data);
        }
        [Route("save")]
        public NAACAlumniMeetingDTO save([FromBody] NAACAlumniMeetingDTO data)
        {
            return _Interface.save(data);
        }

        [Route("deactiveStudent")]
        public NAACAlumniMeetingDTO deactiveStudent([FromBody] NAACAlumniMeetingDTO data)
        {
            return _Interface.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAACAlumniMeetingDTO EditData([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.EditData(category);
        }
            [Route("viewuploadflies")]
        public NAACAlumniMeetingDTO viewuploadflies([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.viewuploadflies(category);
        }
            [Route("deleteuploadfile")]
        public NAACAlumniMeetingDTO deleteuploadfile([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.deleteuploadfile(category);
        }
        [Route("savemedicaldatawisecomments")]
        public NAACAlumniMeetingDTO savemedicaldatawisecomments([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.savemedicaldatawisecomments(category);
        }
        [Route("getcomment")]
        public NAACAlumniMeetingDTO getcomment([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.getcomment(category);
        }
        [Route("getfilecomment")]
        public NAACAlumniMeetingDTO getfilecomment([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.getfilecomment(category);
        }
        [Route("savefilewisecomments")]
        public NAACAlumniMeetingDTO savefilewisecomments([FromBody]NAACAlumniMeetingDTO category)
        {
            return _Interface.savefilewisecomments(category);
        }
    }
}
