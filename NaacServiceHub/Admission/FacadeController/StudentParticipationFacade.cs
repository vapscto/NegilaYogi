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
    public class StudentParticipationFacade : Controller
    {
        public StudentParticipationInterface _Iobj;
        public StudentParticipationFacade(StudentParticipationInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_SParticipation_123_Students_DTO> loaddata([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_AC_SParticipation_123_Students_DTO savedata([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.savedata(data);
        }

        [Route("editdata")]
        public Task<NAAC_AC_SParticipation_123_Students_DTO> editdata([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.editdata(data);
        }

        [Route("deactivY")]
        public Task<NAAC_AC_SParticipation_123_Students_DTO> deactivY([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.deactivY(data);
        }           

        [Route("get_branch")]
        public NAAC_AC_SParticipation_123_Students_DTO get_branch([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.get_branch(data);
        }


        [Route("get_student")]
        public NAAC_AC_SParticipation_123_Students_DTO get_student([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.get_student(data);
        }


        [Route("get_MappedStudentList")]
        public Task<NAAC_AC_SParticipation_123_Students_DTO> get_MappedStudentList([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.get_MappedStudentList(data);
        }

        [Route("viewuploadflies")]
        public NAAC_AC_SParticipation_123_Students_DTO viewuploadflies([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_SParticipation_123_Students_DTO deleteuploadfile([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("get_coursebrnch")]
        public Task<NAAC_AC_SParticipation_123_Students_DTO> get_coursebrnch([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.get_coursebrnch(data);
        }

        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_SParticipation_123_Students_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_SParticipation_123_Students_DTO savefilewisecomments([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_SParticipation_123_Students_DTO getcomment([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_SParticipation_123_Students_DTO getfilecomment([FromBody] NAAC_AC_SParticipation_123_Students_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }

    }
}
