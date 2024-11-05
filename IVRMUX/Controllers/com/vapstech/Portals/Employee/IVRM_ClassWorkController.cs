using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using Microsoft.AspNetCore.Http;
using corewebapi18072016.Delegates.com.vapstech.Portals.Employee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers.com.vapstech.Portals.Employee
{
    [Route("api/[controller]")]
    public class IVRM_ClassWorkController : Controller
    {
        IVRM_ClassWorkDelegats _notic = new IVRM_ClassWorkDelegats();
        // GET: api/values
        [Route("savedetail")]
        public IVRM_ClassWorkDTO savedetail([FromBody]IVRM_ClassWorkDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.MI_Id = mid;
            return _notic.savedetail(data);
        }

        [Route("Getdetails/{id:int}")]
        public IVRM_ClassWorkDTO Getdetails(int id)
        {
            IVRM_ClassWorkDTO obj = new IVRM_ClassWorkDTO();
            // id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            obj.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            obj.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));

            return _notic.Getdetails(obj);
        }
        [Route("deactivate")]
        public IVRM_ClassWorkDTO deactivate([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.deactivate(data);
        }
        [Route("get_classes")]
        public IVRM_ClassWorkDTO get_classes([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.AMST_Id = Convert.ToInt32(HttpContext.Session.GetInt32("AMST_Id"));
            //      data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Role_flag = Convert.ToString(HttpContext.Session.GetString("Roleflag"));
            data.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.get_classes(data);
        }
        [Route("getsectiondata")]
        public IVRM_ClassWorkDTO getsectiondata([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsectiondata(data);
        }
        [Route("getsubject")]
        public IVRM_ClassWorkDTO getsubject([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));

            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsubject(data);
        }

        [Route("editData")]
        public IVRM_ClassWorkDTO editData([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.editData(data);
        }

        [Route("viewData")]
        public IVRM_ClassWorkDTO viewData([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.viewData(data);
        }

        //===================Class work marks enter=========


        [Route("getclasswork_student")]
        public IVRM_ClassWorkDTO getclasswork_student([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getclasswork_student(data);
        }

        [Route("getsubjectlist")]
        public IVRM_ClassWorkDTO getsubjectlist([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getsubjectlist(data);
        }
        [Route("getclasswork_list")]
        public IVRM_ClassWorkDTO getclasswork_list([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getclasswork_list(data);
        }
        [Route("classwork_marks_update")]
        public IVRM_ClassWorkDTO classwork_marks_update([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));


            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.classwork_marks_update(data);
        }
        [Route("edit_classwork_mark")]
        public IVRM_ClassWorkDTO edit_classwork_mark([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.edit_classwork_mark(data);
        }
        [Route("viewclasswork")]
        public IVRM_ClassWorkDTO viewclasswork([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.viewclasswork(data);
        }
        [Route("viewstudentupload")]
        public IVRM_ClassWorkDTO viewstudentupload([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            return _notic.viewstudentupload(data);
        }
        [Route("stfupload")]
        public IVRM_ClassWorkDTO stfupload([FromBody] IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Login_Id = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.stfupload(data);
        }
        //noticeboard consolidated report
        [Route("Getdata_class/{id:int}")]
        public IVRM_ClassWorkDTO Getdata_class(int id)
        {
            IVRM_ClassWorkDTO dto = new IVRM_ClassWorkDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.Getdata_class(dto);
        }
        [Route("getreportnotice")]
        public IVRM_ClassWorkDTO getreportnotice([FromBody]IVRM_ClassWorkDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            // data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _notic.getreportnotice(data);
        }

        [Route("Getdataview")]
        public IVRM_ClassWorkDTO Getdataview([FromBody] IVRM_ClassWorkDTO dto)
        {
            // HomeWorkUploadDTO dto = new HomeWorkUploadDTO();
            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            // dto.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _notic.Getdataview(dto);
        }
        [Route("getclasswork_Topiclist")]
        public IVRM_ClassWorkDTO getclasswork_Topiclist([FromBody] IVRM_ClassWorkDTO dto)
        {

            dto.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.IVRMRT_Id = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            dto.Login_Id = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            return _notic.getclasswork_Topiclist(dto);
        }
    }
}