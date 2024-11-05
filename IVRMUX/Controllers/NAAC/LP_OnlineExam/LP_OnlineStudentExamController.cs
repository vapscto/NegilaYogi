using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.LP_OnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.LP_OnlineExam
{
    [Route("api/[controller]")]     
    public class LP_OnlineStudentExamController : Controller
    {
        LP_OnlineStudentExamDelegate _delg = new LP_OnlineStudentExamDelegate();

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [Route("getloaddata/{id:int}")]
        public LP_OnlineStudentExamDTO getloaddata(int id)
        {
            LP_OnlineStudentExamDTO data = new LP_OnlineStudentExamDTO();
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getloaddata(data);
        }

        [Route("onselectsubject")]
        public LP_OnlineStudentExamDTO onselectsubject([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.onselectsubject(data);
        }

        [Route("getQuestion")]
        public LP_OnlineStudentExamDTO getQuestion([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getQuestion(data);
        }

        [Route("Saveanswer")]
        public LP_OnlineStudentExamDTO Saveanswer([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.Saveanswer(data);
        }

        [Route("savedanswers")]
        public LP_OnlineStudentExamDTO savedanswers([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.savedanswers(data);
        }

        [Route("submitexam")]
        public LP_OnlineStudentExamDTO submitexam([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.submitexam(data);
        }

        [Route("GetViewMarks")]
        public LP_OnlineStudentExamDTO GetViewMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetViewMarks(data);
        }

        [Route("SaveAnswerSheet")]
        public LP_OnlineStudentExamDTO SaveAnswerSheet([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.ASMAY_Id = Convert.ToInt64(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.SaveAnswerSheet(data);
        }

        // Report
        [Route("getloaddatareport/{id:int}")]
        public LP_OnlineStudentExamDTO getloaddatareport(int id)
        {
            LP_OnlineStudentExamDTO data = new LP_OnlineStudentExamDTO();

            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getloaddatareport(data);
        }

        [Route("onchangeyear")]
        public LP_OnlineStudentExamDTO onchangeyear([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public LP_OnlineStudentExamDTO onchangeclass([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangeclass(data);
        }

        [Route("OnchangeSection")]
        public LP_OnlineStudentExamDTO OnchangeSection([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnchangeSection(data);
        }

        [Route("onchangesubject")]
        public LP_OnlineStudentExamDTO onchangesubject([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubject(data);
        }

        [Route("onchangesubject_studentmarks")]
        public LP_OnlineStudentExamDTO onchangesubject_studentmarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.onchangesubject_studentmarks(data);
        }

        [Route("OnChangeExam")]
        public LP_OnlineStudentExamDTO OnChangeExam([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.OnChangeExam(data);
        }

        [Route("getreport")]
        public LP_OnlineStudentExamDTO getreport([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getreport(data);
        }

        [Route("ViewStudentWiseMarks")]
        public LP_OnlineStudentExamDTO ViewStudentWiseMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewStudentWiseMarks(data);
        }

        // Answer Sheet Correction

        [Route("GetSearchDetails")]
        public LP_OnlineStudentExamDTO GetSearchDetails([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetSearchDetails(data);
        }

        [Route("SaveMarks")]
        public LP_OnlineStudentExamDTO SaveMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveMarks(data);
        }

        [Route("SaveStudentAnswerFileByStaff")]
        public LP_OnlineStudentExamDTO SaveStudentAnswerFileByStaff([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveStudentAnswerFileByStaff(data);
        }

        // Subjective Marks
        [Route("ViewQuestion")]
        public LP_OnlineStudentExamDTO ViewQuestion([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.ViewQuestion(data);
        }

        [Route("SaveSubjectiveMarks")]
        public LP_OnlineStudentExamDTO SaveSubjectiveMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.SaveSubjectiveMarks(data);
        }

        [Route("PublishToStudent")]
        public LP_OnlineStudentExamDTO PublishToStudent([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.PublishToStudent(data);
        }

        // Pushing Online Exam Marks To Master Exam Marks

        [Route("GetStudentListForPublish")]
        public LP_OnlineStudentExamDTO GetStudentListForPublish([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetStudentListForPublish(data);
        }

        [Route("CheckStudentMarksEntered")]
        public LP_OnlineStudentExamDTO CheckStudentMarksEntered([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.CheckStudentMarksEntered(data);
        }

        [Route("GetExam_OE_StudentList")]
        public LP_OnlineStudentExamDTO GetExam_OE_StudentList([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.GetExam_OE_StudentList(data);
        }

        [Route("SaveOE_Marks_ME")]
        public LP_OnlineStudentExamDTO SaveOE_Marks_ME([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            data.IP4 = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return _delg.SaveOE_Marks_ME(data);
        }

        //online exam not submitted list report

        [Route("getNonSubmittedreport")]
        public LP_OnlineStudentExamDTO getNonSubmittedreport([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.getNonSubmittedreport(data);
        }

        [Route("MergeFiles")]
        public LP_OnlineStudentExamDTO MergeFiles([FromBody] LP_OnlineStudentExamDTO data)
        {
            data.MI_Id = Convert.ToInt64(HttpContext.Session.GetInt32("Session_MI_Id"));            
            data.UserId = Convert.ToInt64(HttpContext.Session.GetInt32("UserId"));
            return _delg.MergeFiles(data);
        }
    }
}
