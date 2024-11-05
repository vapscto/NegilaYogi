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
    public class NaacEContent434Facade : Controller
    {
        public NAAC_EContent_434_Interface _InterFace;
        public NaacEContent434Facade(NAAC_EContent_434_Interface para)
        {
            _InterFace = para;
        }

        [Route("loaddata")]
        public NAAC_AC_434_EContent_DTO loaddata([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_AC_434_EContent_DTO savedata([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.savedata(data);
        }

        [Route("getcomment")]
        public NAAC_AC_434_EContent_DTO getcomment([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.getcomment(data);
        }

        [Route("getfilecomment")]
        public NAAC_AC_434_EContent_DTO getfilecomment([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.getfilecomment(data);
        }

        [Route("savefilewisecomments")]
        public NAAC_AC_434_EContent_DTO savefilewisecomments([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.savefilewisecomments(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_434_EContent_DTO savemedicaldatawisecomments([FromBody]NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.savemedicaldatawisecomments(data);
        }

        [Route("editdata")]
        public NAAC_AC_434_EContent_DTO editdata([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.editdata(data);
        }

        [Route("deactiveStudent")]
        public NAAC_AC_434_EContent_DTO deactiveStudent([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.deactiveStudent(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_434_EContent_DTO viewuploadflies([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.viewuploadflies(data);
        }

        [Route("deleteuploadfile")]
        public NAAC_AC_434_EContent_DTO deleteuploadfile([FromBody] NAAC_AC_434_EContent_DTO data)
        {
            return _InterFace.deleteuploadfile(data);
        }
    }
}
