using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates.com.vapstech.admission;
using PreadmissionDTOs.com.vaps.admission;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Admission.com.vapstech.controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class StudentTCController : Controller
    {
        private StudentTCDelegate _StuDTO = new StudentTCDelegate();
        // GET: api/StudentTC

        [Route("getstudentdata")]
        public StudentTCDTO LoadInitialData([FromBody]StudentTCDTO ID)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            ID.MI_Id = MIId;
            ID.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            ID.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));

            return _StuDTO.LoadInitialData(ID);
        }

        [Route("gettcdetails")]
        public StudentTCDTO getTcDetails([FromBody] StudentTCDTO dto_obj)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto_obj.MI_Id = MIId;
            if (dto_obj.allorindividual == "All")
            {
                dto_obj.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            dto_obj.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            dto_obj = _StuDTO.getTcDetails(dto_obj);
            return dto_obj;
        }

        [Route("getstudentactivestatus")]
        public StudentTCDTO getstudentactivestatus([FromBody]StudentTCDTO id)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            id.MI_Id = MIId;
            id.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.getActiveDetails(id);
        }

        [Route("get_student_status")]
        public StudentTCDTO get_student_status([FromBody]StudentTCDTO status)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            status.MI_Id = MIId;
            status.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.getStatusDetails(status);
        }

        [Route("savedata")]
        public StudentTCDTO Post([FromBody]StudentTCDTO tc)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            if (tc.allorindividual == "All")
            {
                tc.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            tc.MI_Id = MIId;
            tc.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.savedetails(tc);
        }

        [Route("chk_dup_tc")]
        public StudentTCDTO chk_dup_tc([FromBody]StudentTCDTO tc_no)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            tc_no.MI_Id = MIId;
            tc_no.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.chk_tc_dup(tc_no);
        }

        [Route("saveOtherDetails")]
        public StudentTCDTO OtherDetails([FromBody]StudentTCDTO ot_det)
        {
            ot_det.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.saveOtherdetails(ot_det);
        }

        [Route("getstudent_name_list")]
        public StudentTCDTO getstudent_name_list([FromBody]StudentTCDTO name_list_data)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            name_list_data.MI_Id = MIId;

            name_list_data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            name_list_data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.getstudent_name_list(name_list_data);
        }

        [Route("searchfilter")]
        public StudentTCDTO searchfilter([FromBody] StudentTCDTO data)
        {
            if (data.allorindividual == "All")
            {
                data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            }
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.searchfilter(data);
        }

        // TC Cancel
        [Route("GetTCCancelDetails/{id:int}")]
        public StudentTCDTO GetTCCancelDetails(int id)
        {
            StudentTCDTO data = new StudentTCDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id")); 
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.GetTCCancelDetails(data);
        }
        [Route("OnChangeAcademicYear")]
        public StudentTCDTO OnChangeAcademicYear([FromBody] StudentTCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.OnChangeAcademicYear(data);
        }
        [Route("OnStudentNameChange")]
        public StudentTCDTO OnStudentNameChange([FromBody] StudentTCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.OnStudentNameChange(data);
        }
        [Route("SaveTCCancelDetails")]
        public StudentTCDTO SaveTCCancelDetails([FromBody] StudentTCDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.SaveTCCancelDetails(data);
        }

        //sourcewise admission count 
        [HttpPost]
        [Route("sourcecntdata")]
     
        public StudentTCDTO sourcecntdata([FromBody]StudentTCDTO status)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            status.MI_Id = MIId;
            status.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.sourcecntdata(status);
        }
        [Route("getallsourcedetails")]

        public StudentTCDTO getallsourcedetails(StudentTCDTO status)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            status.MI_Id = MIId;
            status.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.getallsourcedetails(status);
        }
        //MotherTonguewise admission count 
        [Route("languagecntdata")]

        public StudentTCDTO languagecntdata([FromBody]StudentTCDTO status)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            status.MI_Id = MIId;
            status.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.languagecntdata(status);
        }
        //StateWise
        [Route("statecntdata")]

        public StudentTCDTO statecntdata([FromBody]StudentTCDTO status)
        {
            long MIId = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            status.MI_Id = MIId;
            status.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _StuDTO.statecntdata(status);
        }

    }
}
