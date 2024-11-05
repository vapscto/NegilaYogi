using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.Medical.Interface;
using PreadmissionDTOs.NAAC.Medical;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.Medical.FacadeController
{
    [Route("api/[controller]")]
    public class MC_121_IntDept_CourseFacade : Controller
    {
        public MC_121_IntDept_CourseInterface _Interface;

        public MC_121_IntDept_CourseFacade(MC_121_IntDept_CourseInterface parameter)
        {
            _Interface = parameter;
        }

        [Route("loaddata")]
        public MC_121_IntDept_Course_DTO loaddata([FromBody] MC_121_IntDept_Course_DTO data)
        {

            return _Interface.loaddata(data);
        }


        [Route("savedata")]
        public MC_121_IntDept_Course_DTO savedata([FromBody] MC_121_IntDept_Course_DTO data)
        {


            return _Interface.savedata(data);
        }

        [Route("editdata")]
        public MC_121_IntDept_Course_DTO editdata([FromBody] MC_121_IntDept_Course_DTO data)
        {


            return _Interface.editdata(data);
        }

        [Route("deactivY")]
        public MC_121_IntDept_Course_DTO deactivY([FromBody] MC_121_IntDept_Course_DTO data)
        {


            return _Interface.deactivY(data);
        }

        [Route("get_Course")]
        public Task<MC_121_IntDept_Course_DTO> get_Course([FromBody] MC_121_IntDept_Course_DTO data)
        {

            return _Interface.get_Course(data);
        }

        [Route("viewuploadflies")]
        public MC_121_IntDept_Course_DTO viewuploadflies([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.viewuploadflies(data);
        }
        [Route("deleteuploadfile")]
        public MC_121_IntDept_Course_DTO deleteuploadfile([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.deleteuploadfile(data);
        }



        [Route("savemedicaldatawisecomments")]
        public MC_121_IntDept_Course_DTO savemedicaldatawisecomments([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public MC_121_IntDept_Course_DTO savefilewisecomments([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.savefilewisecomments(data);
        }
        [Route("getcomment")]
        public MC_121_IntDept_Course_DTO getcomment([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.getcomment(data);
        }
        [Route("getfilecomment")]
        public MC_121_IntDept_Course_DTO getfilecomment([FromBody] MC_121_IntDept_Course_DTO data)
        {
            return _Interface.getfilecomment(data);
        }

    }
}
