using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.Admission;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.Admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.Admission
{
    [Route("api/[controller]")]
    public class NaacCommiteeController : Controller
    {
        NaacCommiteeDelegate del = new NaacCommiteeDelegate();

        [Route("loaddata/{id:int}")]
        public NAAC_AC_Committee_DTO loaddata(int id)
        {
            NAAC_AC_Committee_DTO data = new NAAC_AC_Committee_DTO();
            data.MI_Id = id;
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.loaddata(data);
        }
        [Route("saverecord")]
        public NAAC_AC_Committee_DTO saverecord([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.saverecord(data);
        }
        [Route("get_Designation")]
        public NAAC_AC_Committee_DTO get_Designation([FromBody] NAAC_AC_Committee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Designation(data);
        }
        [Route("get_Employee")]
        public NAAC_AC_Committee_DTO get_Employee([FromBody] NAAC_AC_Committee_DTO data)
        {
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_Employee(data);
        }

        [Route("getcomment")]
        public NAAC_AC_Committee_DTO getcomment([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcomment(data);
        }
        [Route("getfilecomment")]
        public NAAC_AC_Committee_DTO getfilecomment([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecomment(data);
        }
        [Route("savefilewisecomments")]
        public NAAC_AC_Committee_DTO savefilewisecomments([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecomments(data);
        }
        [Route("savemedicaldatawisecomments")]
        public NAAC_AC_Committee_DTO savemedicaldatawisecomments([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecomments(data);
        }
        [Route("deactiveStudent")]
        public NAAC_AC_Committee_DTO deactiveStudent([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactiveStudent(data);
        }
        [Route("getcommentmember")]
        public NAAC_AC_Committee_DTO getcommentmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getcommentmember(data);
        }
        [Route("savemedicaldatawisecommentsmember")]
        public NAAC_AC_Committee_DTO savemedicaldatawisecommentsmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savemedicaldatawisecommentsmember(data);
        }
        [Route("EditData")]
        public NAAC_AC_Committee_DTO EditData([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.EditData(data);
        }
        [Route("getfilecommentmember")]
        public NAAC_AC_Committee_DTO getfilecommentmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.getfilecommentmember(data);
        }
        [Route("savefilewisecommentsmember")]
        public NAAC_AC_Committee_DTO savefilewisecommentsmember([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.savefilewisecommentsmember(data);
        }
        [Route("get_MappedStaff")]
        public NAAC_AC_Committee_DTO get_MappedStaff([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.get_MappedStaff(data);
        }
        [Route("deactive_staff")]
        public NAAC_AC_Committee_DTO deactive_staff([FromBody] NAAC_AC_Committee_DTO data)
        {
           // data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.deactive_staff(data);
        }
        [Route("viewdocument_MainActUploadFiles")]
        public NAAC_AC_Committee_DTO viewdocument_MainActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_MainActUploadFiles(data);
        }

        [Route("delete_MainActUploadFiles")]
        public NAAC_AC_Committee_DTO delete_MainActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_MainActUploadFiles(data);
        }

        [Route("viewdocument_StaffActUploadFiles")]
        public NAAC_AC_Committee_DTO viewdocument_StaffActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.viewdocument_StaffActUploadFiles(data);
        }

        [Route("delete_StaffActUploadFiles")]
        public NAAC_AC_Committee_DTO delete_StaffActUploadFiles([FromBody] NAAC_AC_Committee_DTO data)
        {
            //data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return del.delete_StaffActUploadFiles(data);
        }
    }
}
