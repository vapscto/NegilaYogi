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
    public class Naac_VACFacade : Controller
    {


        public Naac_VACInterface _Iobj;
        public Naac_VACFacade(Naac_VACInterface para)
        {
            _Iobj = para;
        }

        [Route("loaddata")]
        public Task<NAAC_AC_VAC_DTO> loaddata([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.loaddata(data);
        }
        [Route("savedatatab1")]
        public NAAC_AC_VAC_DTO savedatatab1([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savedatatab1(data);
        }
        [Route("getcommentmaster")]
        public NAAC_AC_VAC_DTO getcommentmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.getcommentmaster(data);
        }
        [Route("savemedicaldatawisecommentsmaster")]
        public NAAC_AC_VAC_DTO savemedicaldatawisecommentsmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savemedicaldatawisecommentsmaster(data);
        }
        [Route("savefilewisecommentsmaster")]
        public NAAC_AC_VAC_DTO savefilewisecommentsmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savefilewisecommentsmaster(data);
        }
        [Route("getfilecommentmaster")]
        public NAAC_AC_VAC_DTO getfilecommentmaster([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.getfilecommentmaster(data);
        }
        [Route("editTab1")]
        public NAAC_AC_VAC_DTO editTab1([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.editTab1(data);
        }

        [Route("deactivYTab1")]
        public NAAC_AC_VAC_DTO deactivYTab1([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.deactivYTab1(data);
        }

        [Route("get_student")]
        public NAAC_AC_VAC_DTO get_student([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.get_student(data);
        }
        [Route("savedatatab2")]
        public NAAC_AC_VAC_DTO savedatatab2([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savedatatab2(data);
        }
        [Route("getcomment")]
        public NAAC_AC_VAC_DTO getcomment([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_VAC_DTO getfilecomment([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_VAC_DTO savefilewisecomments([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savefilewisecomments(data);
        }
        [Route("deactivYTabstudent")]
        public NAAC_AC_VAC_DTO deactivYTabstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.deactivYTabstudent(data);
        }
        [Route("viewstudent")]
        public NAAC_AC_VAC_DTO viewstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.viewstudent(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_VAC_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.savemedicaldatawisecomments(data);
        }

        [Route("edittab2")]
        public NAAC_AC_VAC_DTO edittab2([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.edittab2(data);
        }

        [Route("deactivYTab2")]
        public NAAC_AC_VAC_DTO deactivYTab2([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.deactivYTab2(data);
        }

        [Route("get_Mappedstudentlist")]
        public NAAC_AC_VAC_DTO get_Mappedstudentlist([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.get_Mappedstudentlist(data);
        }

        [Route("get_Continuedflagdata")]
        public NAAC_AC_VAC_DTO get_Continuedflagdata([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.get_Continuedflagdata(data);
        }
        [Route("saveContinued")]
        public NAAC_AC_VAC_DTO saveContinued([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.saveContinued(data);
        }
        [Route("get_Completedflagdata")]
        public NAAC_AC_VAC_DTO get_Completedflagdata([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.get_Completedflagdata(data);
        }
        [Route("saveCompletedflag")]
        public NAAC_AC_VAC_DTO saveCompletedflag([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.saveCompletedflag(data);
        }
        [Route("viewuploadfliesmain")]
        public NAAC_AC_VAC_DTO viewuploadfliesmain([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.viewuploadfliesmain(data);
        }
        [Route("deletemainfile")]
        public NAAC_AC_VAC_DTO deletemainfile([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.deletemainfile(data);
        }
        [Route("viewuploadfliesstudent")]
        public NAAC_AC_VAC_DTO viewuploadfliesstudent([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.viewuploadfliesstudent(data);
        }
        [Route("deletestudentfiles")]
        public NAAC_AC_VAC_DTO deletestudentfiles([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.deletestudentfiles(data);
        }
        //added by sanjeev
        [Route("saveadvance")]
        public NAAC_AC_VAC_DTO saveadvance([FromBody] NAAC_AC_VAC_DTO data)
        {
            return _Iobj.saveadvance(data);
        }
    }
}
