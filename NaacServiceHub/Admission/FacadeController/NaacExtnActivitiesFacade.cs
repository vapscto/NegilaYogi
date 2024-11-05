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
    public class NaacExtnActivitiesFacade : Controller
    {
        public NaacExtnActivitiesInterface inter;

        public NaacExtnActivitiesFacade(NaacExtnActivitiesInterface w)
        {
            inter = w;
        }

        [Route("loaddata")]
        public NAAC_AC_344_ExtnActivities_DTO loaddata([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("get_branch")]
        public NAAC_AC_344_ExtnActivities_DTO get_branch([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_branch(data);
        }
        [Route("get_sems")]

        public NAAC_AC_344_ExtnActivities_DTO get_sems([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_sems(data);
        }

        [Route("get_section")]
        public NAAC_AC_344_ExtnActivities_DTO get_section([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_section(data);
        }

        [Route("GetStudentDetails")]
        public NAAC_AC_344_ExtnActivities_DTO GetStudentDetails([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.GetStudentDetails(data);
        }
        [Route("saverecord")]
        public NAAC_AC_344_ExtnActivities_DTO saverecord([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.saverecord(data);
        }
        [Route("getcomment")]
        public NAAC_AC_344_ExtnActivities_DTO getcomment([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_344_ExtnActivities_DTO getfilecomment([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_344_ExtnActivities_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_344_ExtnActivities_DTO savefilewisecomments([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.savefilewisecomments(data);
        }

        [Route("deactiveStudent")]
        public NAAC_AC_344_ExtnActivities_DTO deactiveStudent([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.deactiveStudent(data);
        }

        [Route("EditData")]
        public NAAC_AC_344_ExtnActivities_DTO EditData([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("get_MappedStudent")]
        public Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStudent([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_MappedStudent(data);
        }

        [Route("deactive_student")]
        public NAAC_AC_344_ExtnActivities_DTO deactive_student([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.deactive_student(data);
        }

        [Route("viewdocument_MainActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_MainActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_MainActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.delete_MainActUploadFiles(data);
        }

        [Route("viewdocument_StudentActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StudentActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.viewdocument_StudentActUploadFiles(data);
        }

        [Route("delete_StudentActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_StudentActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.delete_StudentActUploadFiles(data);
        }

        [Route("get_Designation")]
        public NAAC_AC_344_ExtnActivities_DTO get_Designation([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_Designation(data);
        }

        [Route("get_Employee")]
        public NAAC_AC_344_ExtnActivities_DTO get_Employee([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_Employee(data);
        }

        [Route("viewdocument_StaffActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO viewdocument_StaffActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NAAC_AC_344_ExtnActivities_DTO delete_StaffActUploadFiles([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.delete_StaffActUploadFiles(data);
        }

        [Route("get_MappedStaff")]
        public Task<NAAC_AC_344_ExtnActivities_DTO> get_MappedStaff([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {
            return inter.get_MappedStaff(data);
        }

        [Route("deactive_staff")]
        public NAAC_AC_344_ExtnActivities_DTO deactive_staff([FromBody] NAAC_AC_344_ExtnActivities_DTO data)
        {

            return inter.deactive_staff(data);
        }
    }
}
