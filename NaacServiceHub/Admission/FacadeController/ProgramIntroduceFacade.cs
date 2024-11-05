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
    public class ProgramIntroduceFacade : Controller
    {
        public ProgramIntroduceInterface _Iobj;
        public ProgramIntroduceFacade(ProgramIntroduceInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_Programs_112_DTO> loaddata([FromBody] NAAC_AC_Programs_112_DTO data)
        {            
            return _Iobj.loaddata(data);
        }

        [Route("savedata")]
        public NAAC_AC_Programs_112_DTO savedata([FromBody] NAAC_AC_Programs_112_DTO data)
        {            
            return _Iobj.savedata(data);
        }

        [Route("editdata")]
        public NAAC_AC_Programs_112_DTO editdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {           
            return _Iobj.editdata(data);
        }

        [Route("deactivY")]
        public NAAC_AC_Programs_112_DTO deactivY([FromBody] NAAC_AC_Programs_112_DTO data)
        {            
            return _Iobj.deactivY(data);
        }

        [Route("get_Discontinuedflagdata")]
        public NAAC_AC_Programs_112_DTO get_Discontinuedflagdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.get_Discontinuedflagdata(data);
        }
        [Route("saveContinued")]
        public NAAC_AC_Programs_112_DTO saveContinued([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.saveContinued(data);
        }
        [Route("savemappingdata")]
        public NAAC_AC_Programs_112_DTO savemappingdata([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.savemappingdata(data);
        }
        [Route("deactivYTab2")]
        public NAAC_AC_Programs_112_DTO deactivYTab2([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.deactivYTab2(data);
        }
        [Route("edittab2")]
        public NAAC_AC_Programs_112_DTO edittab2([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.edittab2(data);
        }
        [Route("viewuploadflies")]
        public NAAC_AC_Programs_112_DTO viewuploadflies([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public NAAC_AC_Programs_112_DTO deleteuploadfile([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.deleteuploadfile(data);
        }
        [Route("get_branch")]
        public NAAC_AC_Programs_112_DTO get_branch([FromBody] NAAC_AC_Programs_112_DTO data)
        {         
            return _Iobj.get_branch(data);
        }
        [Route("get_program")]
        public NAAC_AC_Programs_112_DTO get_program([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.get_program(data);
        }
        [Route("get_Course")]
        public Task<NAAC_AC_Programs_112_DTO> get_Course([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.get_Course(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_Programs_112_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_Programs_112_DTO savefilewisecomments([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public NAAC_AC_Programs_112_DTO getcomment([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_Programs_112_DTO getfilecomment([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        //adedd by sanjeeev
        
        [Route("saveadvance")]
        public NAAC_AC_Programs_112_DTO saveadvance([FromBody] NAAC_AC_Programs_112_DTO data)
        {
            return _Iobj.saveadvanceAsync(data);
        }
    }
}
