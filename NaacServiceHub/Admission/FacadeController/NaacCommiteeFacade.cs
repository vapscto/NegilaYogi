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
    public class NaacCommiteeFacade : Controller
    {

        public NaacCommiteeInterface inter;
        public NaacCommiteeFacade(NaacCommiteeInterface o)
        {
            inter = o;
        }


        [Route("loaddata")]
        public NAAC_AC_Committee_DTO loaddata([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.loaddata(data);
        }
        [Route("saverecord")]
        public NAAC_AC_Committee_DTO saverecord([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.saverecord(data);
        }
        [Route("get_Designation")]
        public NAAC_AC_Committee_DTO get_Designation([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.get_Designation(data);
        }

        [Route("getcomment")]
        public NAAC_AC_Committee_DTO getcomment([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.getcomment(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_Committee_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.savemedicaldatawisecomments(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_Committee_DTO savefilewisecomments([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.savefilewisecomments(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_Committee_DTO getfilecomment([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.getfilecomment(data);
        }
        [Route("get_Employee")]
        public NAAC_AC_Committee_DTO get_Employee([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.get_Employee(data);
        }
        [Route("getfilecommentmember")]
        public NAAC_AC_Committee_DTO getfilecommentmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.getfilecommentmember(data);
        }
        [Route("savefilewisecommentsmember")]
        public NAAC_AC_Committee_DTO savefilewisecommentsmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.savefilewisecommentsmember(data);
        }
        [Route("savemedicaldatawisecommentsmember")]
        public NAAC_AC_Committee_DTO savemedicaldatawisecommentsmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.savemedicaldatawisecommentsmember(data);
        }
        [Route("getcommentmember")]
        public NAAC_AC_Committee_DTO getcommentmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.getcommentmember(data);
        }
        [Route("deactiveStudent")]
        public NAAC_AC_Committee_DTO deactiveStudent([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.deactiveStudent(data);
        }
        [Route("EditData")]
        public NAAC_AC_Committee_DTO EditData([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.EditData(data);
        }
        [Route("get_MappedStaff")]
        public Task<NAAC_AC_Committee_DTO> get_MappedStaff([FromBody]NAAC_AC_Committee_DTO data)
        {
            return inter.get_MappedStaff(data);
        }
        [Route("deactive_staff")]
        public NAAC_AC_Committee_DTO deactive_staff([FromBody]NAAC_AC_Committee_DTO data)
        {
            return inter.deactive_staff(data);
        }
        [Route("viewdocument_MainActUploadFiles")]
        public NAAC_AC_Committee_DTO viewdocument_MainActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NAAC_AC_Committee_DTO delete_MainActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.delete_MainActUploadFiles(data);
        }
        [Route("viewdocument_StaffActUploadFiles")]
        public NAAC_AC_Committee_DTO viewdocument_StaffActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NAAC_AC_Committee_DTO delete_StaffActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            return inter.delete_StaffActUploadFiles(data);
        }
    }
}
