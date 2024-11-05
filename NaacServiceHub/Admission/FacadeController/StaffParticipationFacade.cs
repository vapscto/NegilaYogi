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
    public class StaffParticipationFacade : Controller
    {
        public StaffParticipationInterface _Iobj;
        public StaffParticipationFacade(StaffParticipationInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_TParticipation_113_DTO> loaddata([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            return _Iobj.loaddata(data);
        }


        [Route("savedata")]
        public NAAC_AC_TParticipation_113_DTO savedata([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {


            return _Iobj.savedata(data);
        }

        [Route("editdata")]
        public NAAC_AC_TParticipation_113_DTO editdata([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {


            return _Iobj.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_AC_TParticipation_113_DTO deactivY([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {


            return _Iobj.deactivY(data);
        }

        [Route("get_designation")]
        public NAAC_AC_TParticipation_113_DTO get_designation([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            return _Iobj.get_designation(data);
        }

        [Route("get_emp")]
        public NAAC_AC_TParticipation_113_DTO get_emp([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {

            return _Iobj.get_emp(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_TParticipation_113_DTO viewuploadflies([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_TParticipation_113_DTO deleteuploadfile([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }


        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_TParticipation_113_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_TParticipation_113_DTO savefilewisecomments([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_TParticipation_113_DTO getcomment([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_TParticipation_113_DTO getfilecomment([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        //added by sanjeev
        [Route("saveadvance")]
        public NAAC_AC_TParticipation_113_DTO saveadvance([FromBody] NAAC_AC_TParticipation_113_DTO data)
        {
            return _Iobj.saveadvance(data);
        }
        
    }
}
