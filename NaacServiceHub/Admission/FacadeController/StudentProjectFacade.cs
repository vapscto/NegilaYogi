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
    public class StudentProjectFacade : Controller
    {
        public StudentProjectInterface inter;
        public StudentProjectFacade(StudentProjectInterface obj)
        {
            inter = obj;
        }
        [Route("loaddata")]
        public Task<StudentProject_DTO> loaddata([FromBody] StudentProject_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("savedata")]
        public StudentProject_DTO savedata([FromBody] StudentProject_DTO data)
        {
            return inter.savedata(data);
        }

        [Route("editdata")]
        public StudentProject_DTO editdata([FromBody] StudentProject_DTO data)
        {
            return inter.editdata(data);
        }

        [Route("deactiveStudent")]
        public StudentProject_DTO deactiveStudent([FromBody] StudentProject_DTO data)
        {
            return inter.deactiveStudent(data);
        }
        [Route("get_branch")]
        public StudentProject_DTO get_branch([FromBody] StudentProject_DTO data)
        {
            return inter.get_branch(data);
        }
        [Route("get_student")]
        public StudentProject_DTO get_student([FromBody]StudentProject_DTO data)
        {
            return inter.get_student(data);
        }
        [Route("viewuploadflies")]
        public StudentProject_DTO viewuploadflies([FromBody] StudentProject_DTO data)
        {
            return inter.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public StudentProject_DTO deleteuploadfile([FromBody] StudentProject_DTO data)
        {
            return inter.deleteuploadfile(data);
        }



        [Route("MC_Savedata_134")]
        public StudentProject_DTO MC_Savedata_134([FromBody] StudentProject_DTO data)
        {
            return inter.MC_Savedata_134(data);
        }
        [Route("MC_editdata_134")]
        public StudentProject_DTO MC_editdata_134([FromBody]StudentProject_DTO data)
        {
            return inter.MC_editdata_134(data);
        }
        [Route("MC_viewuploadflies_134")]
        public StudentProject_DTO MC_viewuploadflies_134([FromBody] StudentProject_DTO data)
        {
            return inter.MC_viewuploadflies_134(data);
        }
        [Route("MC_deleteuploadfile_134")]
        public StudentProject_DTO MC_deleteuploadfile_134([FromBody] StudentProject_DTO data)
        {
            return inter.MC_deleteuploadfile_134(data);
        }


        [Route("savemedicaldatawisecomments")]
        public StudentProject_DTO savemedicaldatawisecomments([FromBody] StudentProject_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public StudentProject_DTO savefilewisecomments([FromBody] StudentProject_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public StudentProject_DTO getcomment([FromBody] StudentProject_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public StudentProject_DTO getfilecomment([FromBody] StudentProject_DTO data)
        {
            return inter.getfilecomment(data);
        }


        [Route("savedatawisecommentsAffi")]
        public StudentProject_DTO savedatawisecommentsAffi([FromBody] StudentProject_DTO data)
        {
            return inter.savedatawisecommentsAffi(data);
        }
        [Route("savefilewisecommentsAffi")]
        public StudentProject_DTO savefilewisecommentsAffi([FromBody] StudentProject_DTO data)
        {
            return inter.savefilewisecommentsAffi(data);
        }
        [Route("getcommentAffi")]
        public StudentProject_DTO getcommentAffi([FromBody] StudentProject_DTO data)
        {
            return inter.getcommentAffi(data);
        }
        [Route("getfilecommentAffi")]
        public StudentProject_DTO getfilecommentAffi([FromBody] StudentProject_DTO data)
        {
            return inter.getfilecommentAffi(data);
        }


        [Route("deactiveY")]
        public StudentProject_DTO deactiveY([FromBody] StudentProject_DTO data)
        {
            return inter.deactiveY(data);
        }
        //added by sanjeev
        [Route("saveadvance")]
        public StudentProject_DTO saveadvance([FromBody] StudentProject_DTO data)
        {
            return inter.saveadvance(data);
        }
    }
}
