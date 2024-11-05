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
    public class NaacExpenditure424Facade : Controller
    {
        public NaacExpenditure424Interface _InterFace;

        public NaacExpenditure424Facade(NaacExpenditure424Interface q)
        {
            _InterFace = q;
        }

        [Route("save")]
        public NaacExpenditure424DTO save([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.save(data);
        }

        [Route("loaddata")]
        public NaacExpenditure424DTO loaddata([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.loaddata(data);
        }

        [Route("getcomment")]
        public NaacExpenditure424DTO getcomment([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.getcomment(data);
        }

        [Route("getfilecomment")]
        public NaacExpenditure424DTO getfilecomment([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.getfilecomment(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NaacExpenditure424DTO savemedicaldatawisecomments([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.savemedicaldatawisecomments(data);
        }

        [Route("savefilewisecomments")]
        public NaacExpenditure424DTO savefilewisecomments([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.savefilewisecomments(data);
        }

        [Route("deactiveStudent")]
        public NaacExpenditure424DTO deactiveStudent([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.deactiveStudent(data);
        }

        [Route("EditData")]
        public NaacExpenditure424DTO EditData([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.EditData(data);
        }

        [Route("viewuploadflies")]
        public NaacExpenditure424DTO viewuploadflies([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NaacExpenditure424DTO deleteuploadfile([FromBody] NaacExpenditure424DTO data)
        {
            return _InterFace.deleteuploadfile(data);
        }
    }
}
