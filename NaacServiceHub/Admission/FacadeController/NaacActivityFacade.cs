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
    public class NaacActivityFacade : Controller
    {

        public NaacActivityInterface inter;
        public NaacActivityFacade(NaacActivityInterface b)
        {
            inter = b;
        }

        [Route("loaddata")]
        public NaacActivity_DTO loaddata([FromBody] NaacActivity_DTO data)
        {
            return inter.loaddata(data);
        }

        [Route("get_course")]
        public NaacActivity_DTO get_course([FromBody] NaacActivity_DTO data)
        {
            return inter.get_course(data);
        }

        [Route("get_branch")]
        public NaacActivity_DTO get_branch([FromBody] NaacActivity_DTO data)
        {
            return inter.get_branch(data);
        }

        [Route("get_sems")]
        public NaacActivity_DTO get_sems([FromBody] NaacActivity_DTO data)
        {
            return inter.get_sems(data);
        }

        [Route("get_section")]
        public NaacActivity_DTO get_section([FromBody] NaacActivity_DTO data)
        {
            return inter.get_section(data);
        }

        [Route("GetStudentDetails")]
        public NaacActivity_DTO GetStudentDetails([FromBody] NaacActivity_DTO data)
        {
            return inter.GetStudentDetails(data);
        }

        [Route("get_Designation")]
        public NaacActivity_DTO get_Designation([FromBody] NaacActivity_DTO data)
        {
            return inter.get_Designation(data);
        }

        [Route("get_Employee")]
        public NaacActivity_DTO get_Employee([FromBody] NaacActivity_DTO data)
        {
            return inter.get_Employee(data);
        }

        [Route("saverecord")]
        public NaacActivity_DTO saverecord([FromBody] NaacActivity_DTO data)
        {
            return inter.saverecord(data);
        }

        [Route("deactiveStudent")]
        public NaacActivity_DTO deactiveStudent([FromBody] NaacActivity_DTO data)
        {
            return inter.deactiveStudent(data);
        }

        [Route("EditData")]
        public NaacActivity_DTO EditData([FromBody] NaacActivity_DTO data)
        {
            return inter.EditData(data);
        }

        [Route("get_MappedStudent")]
        public Task<NaacActivity_DTO> get_MappedStudent([FromBody] NaacActivity_DTO data)
        {
            return inter.get_MappedStudent(data);
        }

        [Route("get_MappedStaff")]
        public Task<NaacActivity_DTO> get_MappedStaff([FromBody] NaacActivity_DTO data)
        {
            return inter.get_MappedStaff(data);
        }

        [Route("deactive_student")]
        public NaacActivity_DTO deactive_student([FromBody] NaacActivity_DTO data)
        {
            return inter.deactive_student(data);
        }

        [Route("deactive_staff")]
        public NaacActivity_DTO deactive_staff([FromBody] NaacActivity_DTO data)
        {
            return inter.deactive_staff(data);
        }

        [Route("viewdocument_MainActUploadFiles")]
        public NaacActivity_DTO viewdocument_MainActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NaacActivity_DTO delete_MainActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.delete_MainActUploadFiles(data);
        }

        [Route("viewdocument_StudentActUploadFiles")]
        public NaacActivity_DTO viewdocument_StudentActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.viewdocument_StudentActUploadFiles(data);
        }

        [Route("delete_StudentActUploadFiles")]
        public NaacActivity_DTO delete_StudentActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.delete_StudentActUploadFiles(data);
        }

        [Route("viewdocument_StaffActUploadFiles")]
        public NaacActivity_DTO viewdocument_StaffActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NaacActivity_DTO delete_StaffActUploadFiles([FromBody] NaacActivity_DTO data)
        {
            return inter.delete_StaffActUploadFiles(data);
        }

    }
}
