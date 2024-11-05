using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.LP_OnlineExam.Interface;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.LP_OnlineExam.Facade
{
    [Route("api/[controller]")]    
    public class LP_OnlineStudentExamFacadeController : Controller
    {
        LP_OnlineStudentExamInterface _interface;

        public LP_OnlineStudentExamFacadeController(LP_OnlineStudentExamInterface _inter)
        {
            _interface = _inter;
        }
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("getloaddata")]
        public LP_OnlineStudentExamDTO getloaddata([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.getloaddata(data);
        }

        [Route("onselectsubject")]
        public LP_OnlineStudentExamDTO onselectsubject([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.onselectsubject(data);
        }

        [Route("getQuestion")]
        public LP_OnlineStudentExamDTO getQuestion([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.getQuestion(data);
        }

        [Route("Saveanswer")]
        public LP_OnlineStudentExamDTO Saveanswer([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.Saveanswer(data);
        }

        [Route("savedanswers")]
        public LP_OnlineStudentExamDTO savedanswers([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.savedanswers(data);
        }

        [Route("submitexam")]
        public LP_OnlineStudentExamDTO submitexam([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.submitexam(data);
        }
        
        [Route("GetViewMarks")]
        public LP_OnlineStudentExamDTO GetViewMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.GetViewMarks(data);
        }
        
        [Route("SaveAnswerSheet")]
        public LP_OnlineStudentExamDTO SaveAnswerSheet([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.SaveAnswerSheet(data);
        }

        // Report
        [Route("getloaddatareport")]
        public LP_OnlineStudentExamDTO getloaddatareport([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.getloaddatareport(data);
        }

        [Route("onchangeyear")]
        public LP_OnlineStudentExamDTO onchangeyear([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.onchangeyear(data);
        }

        [Route("onchangeclass")]
        public LP_OnlineStudentExamDTO onchangeclass([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.onchangeclass(data);
        }

        [Route("OnchangeSection")]
        public LP_OnlineStudentExamDTO OnchangeSection([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.OnchangeSection(data);
        }

        [Route("onchangesubject")]
        public LP_OnlineStudentExamDTO onchangesubject([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.onchangesubject(data);
        }

        [Route("onchangesubject_studentmarks")]
        public LP_OnlineStudentExamDTO onchangesubject_studentmarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.onchangesubject_studentmarks(data);
        }

        [Route("OnChangeExam")]
        public LP_OnlineStudentExamDTO OnChangeExam([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.OnChangeExam(data);
        }

        [Route("getreport")]
        public LP_OnlineStudentExamDTO getreport([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.getreport(data);
        }

        [Route("ViewStudentWiseMarks")]
        public LP_OnlineStudentExamDTO ViewStudentWiseMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.ViewStudentWiseMarks(data);
        }

        [Route("GetSearchDetails")]
        public LP_OnlineStudentExamDTO GetSearchDetails([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.GetSearchDetails(data);
        }

        [Route("SaveMarks")]
        public LP_OnlineStudentExamDTO SaveMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.SaveMarks(data);
        }

        [Route("SaveStudentAnswerFileByStaff")]
        public LP_OnlineStudentExamDTO SaveStudentAnswerFileByStaff([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.SaveStudentAnswerFileByStaff(data);
        }

        [Route("ViewQuestion")]
        public LP_OnlineStudentExamDTO ViewQuestion([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.ViewQuestion(data);
        }

        [Route("SaveSubjectiveMarks")]
        public LP_OnlineStudentExamDTO SaveSubjectiveMarks([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.SaveSubjectiveMarks(data);
        }

        [Route("PublishToStudent")]
        public LP_OnlineStudentExamDTO PublishToStudent([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.PublishToStudent(data);
        }

        // Pushing Online Exam Marks To Master Exam Marks

        [Route("GetStudentListForPublish")]
        public LP_OnlineStudentExamDTO GetStudentListForPublish([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.GetStudentListForPublish(data);
        }

        [Route("CheckStudentMarksEntered")]
        public LP_OnlineStudentExamDTO CheckStudentMarksEntered([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.CheckStudentMarksEntered(data);
        }

        [Route("GetExam_OE_StudentList")]
        public LP_OnlineStudentExamDTO GetExam_OE_StudentList([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.GetExam_OE_StudentList(data);
        }

        [Route("SaveOE_Marks_ME")]
        public LP_OnlineStudentExamDTO SaveOE_Marks_ME([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.SaveOE_Marks_ME(data);
        }


        //online exam not submitted list report
        [Route("getNonSubmittedreport")]
        public LP_OnlineStudentExamDTO getNonSubmittedreport([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.getNonSubmittedreport(data);
        }
        [Route("MergeFiles")]
        public LP_OnlineStudentExamDTO MergeFiles([FromBody] LP_OnlineStudentExamDTO data)
        {
            return _interface.MergeFiles(data);
        }
    }
}
