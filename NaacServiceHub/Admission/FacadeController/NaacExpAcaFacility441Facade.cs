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
    public class NaacExpAcaFacility441Facade : Controller
    {
        public NaacExpAcaFacility441Interface _InterFace;

        public NaacExpAcaFacility441Facade(NaacExpAcaFacility441Interface q)
        {
            _InterFace = q;
        }

        [Route("loaddata")]
        public NaacExpAcaFacility441DTO loaddata([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.loaddata(data);
        }

        [Route("save")]
        public NaacExpAcaFacility441DTO save([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.save(data);
        }

        [Route("getfilecomment")]
        public NaacExpAcaFacility441DTO getfilecomment([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.getfilecomment(data);
        }

        [Route("savefilewisecomments")]
        public NaacExpAcaFacility441DTO savefilewisecomments([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.savefilewisecomments(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NaacExpAcaFacility441DTO savemedicaldatawisecomments([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.savemedicaldatawisecomments(data);
        }

        [Route("getcomment")]
        public NaacExpAcaFacility441DTO getcomment([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.getcomment(data);
        }

        [Route("deactiveStudent")]
        public NaacExpAcaFacility441DTO deactiveStudent([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.deactiveStudent(data);
        }

        [Route("EditData")]
        public NaacExpAcaFacility441DTO EditData([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.EditData(data);
        }

        [Route("viewuploadflies")]
        public NaacExpAcaFacility441DTO viewuploadflies([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NaacExpAcaFacility441DTO deleteuploadfile([FromBody] NaacExpAcaFacility441DTO data)
        {
            return _InterFace.deleteuploadfile(data);
        }
    }
}
