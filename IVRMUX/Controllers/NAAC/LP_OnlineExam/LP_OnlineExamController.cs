using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVRMUX.Delegates.NAAC.LP_OnlineExam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVRMUX.Controllers.NAAC.LP_OnlineExam
{
    [Route("api/[controller]")]
    public class LP_OnlineExamController : Controller
    {
        LP_OnlineExamDelegate _delg = new LP_OnlineExamDelegate();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // LP ONLINE EXAM CONFIG

        [Route("getconfigloaddata/{id:int}")]
        public LP_OnlineExamDTO getconfigloaddata(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getconfigloaddata(data);
        }

        [Route("saveconfigdata")]
        public LP_OnlineExamDTO saveconfigdata([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.saveconfigdata(data);
        }

        //********** LP SCHOOL ONLINE EXAM MASTER QUESTION  *****************//

        [Route("getmasterquestionloaddata/{id:int}")]
        public LP_OnlineExamDTO getmasterquestionloaddata(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getmasterquestionloaddata(data);
        }

        [Route("getclasslist")]
        public LP_OnlineExamDTO getclasslist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getclasslist(data);
        }

        [Route("getsubjectlist")]
        public LP_OnlineExamDTO getsubjectlist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getsubjectlist(data);
        }

        [Route("gettopiclist")]
        public LP_OnlineExamDTO gettopiclist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.gettopiclist(data);
        }

        [Route("SaveMasterQuestionDetails")]
        public LP_OnlineExamDTO SaveMasterQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.SaveMasterQuestionDetails(data);
        }

        [Route("EditMasterQuestion")]
        public LP_OnlineExamDTO EditMasterQuestion([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.EditMasterQuestion(data);
        }

        [Route("ViewMasterQuesDoc")]
        public LP_OnlineExamDTO ViewMasterQuesDoc([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.ViewMasterQuesDoc(data);
        }

        [Route("DeactivateActivateQuestion")]
        public LP_OnlineExamDTO DeactivateActivateQuestion([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeactivateActivateQuestion(data);
        }   

        [Route("DeactivateActivateDocument")]
        public LP_OnlineExamDTO DeactivateActivateDocument([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeactivateActivateDocument(data);
        }   

        [Route("ViewMasterQuesOptions")]
        public LP_OnlineExamDTO ViewMasterQuesOptions([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.ViewMasterQuesOptions(data);
        }

        [Route("ViewUploadOptionFiles")]
        public LP_OnlineExamDTO ViewUploadOptionFiles([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.ViewUploadOptionFiles(data);
        }   

        [Route("DeactivateActivateQuesOption")]
        public LP_OnlineExamDTO DeactivateActivateQuesOption([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeactivateActivateQuesOption(data);
        }

        [Route("DeactivateActivateOptionsDocument")]
        public LP_OnlineExamDTO DeactivateActivateOptionsDocument([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeactivateActivateOptionsDocument(data);
        }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM  *****************//

        [Route("getexammasterload/{id:int}")]
        public LP_OnlineExamDTO getexammasterload(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getexammasterload(data);
        }

        [Route("getexamclasslist")]
        public LP_OnlineExamDTO getexamclasslist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getexamclasslist(data);
        }

        [Route("getexamsectionslist")]
        public LP_OnlineExamDTO getexamsectionslist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getexamsectionslist(data);
        }

        [Route("getexamsubjectlist")]
        public LP_OnlineExamDTO getexamsubjectlist([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.getexamsubjectlist(data);
        }

        [Route("GetSearchTopics")]
        public LP_OnlineExamDTO GetSearchTopics([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.GetSearchTopics(data);
        }

        [Route("SearchQuestions")]
        public LP_OnlineExamDTO SearchQuestions([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SearchQuestions(data);
        }

        [Route("SaveMasterExamQuestionDetails")]
        public LP_OnlineExamDTO SaveMasterExamQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveMasterExamQuestionDetails(data);
        }

        [Route("EditMasterExamQuestion")]
        public LP_OnlineExamDTO EditMasterExamQuestion([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.EditMasterExamQuestion(data);
        }

        [Route("ViewMasterExamQuesOptions")]
        public LP_OnlineExamDTO ViewMasterExamQuesOptions([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewMasterExamQuesOptions(data);
        }

        [Route("ViewMasterExamLevelDetails")]
        public LP_OnlineExamDTO ViewMasterExamLevelDetails([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewMasterExamLevelDetails(data);
        }

        [Route("ViewSavedLevelQuestons")]
        public LP_OnlineExamDTO ViewSavedLevelQuestons([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewSavedLevelQuestons(data);
        }

        [Route("ViewMasterQuestionExamTopic")]
        public LP_OnlineExamDTO ViewMasterQuestionExamTopic([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewMasterQuestionExamTopic(data);
        }

        [Route("ViewQuestionPaper")]
        public LP_OnlineExamDTO ViewQuestionPaper([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.ViewQuestionPaper(data);
        }

        [Route("DeactivateActivateMasterExam")]
        public LP_OnlineExamDTO DeactivateActivateMasterExam([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateMasterExam(data);
        }

        [Route("DeactivateActivateExamQues")]
        public LP_OnlineExamDTO DeactivateActivateExamQues([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateExamQues(data);
        }

        [Route("DeactivateActivateExamQuesTopic")]
        public LP_OnlineExamDTO DeactivateActivateExamQuesTopic([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.DeactivateActivateExamQuesTopic(data);
        }

        [Route("SearchQuestionfilter")]
        public LP_OnlineExamDTO SearchQuestionfilter([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SearchQuestionfilter(data);
        }

        [Route("OnChangeMasterExam")]
        public LP_OnlineExamDTO OnChangeMasterExam([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.OnChangeMasterExam(data);
        }

        [Route("SaveLevelQuestionOrder")]
        public LP_OnlineExamDTO SaveLevelQuestionOrder([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            return _delg.SaveLevelQuestionOrder(data);
        }

        // Load  Question Deactivate All

        [Route("loaddatadeactivate/{id:int}")]
        public LP_OnlineExamDTO loaddatadeactivate(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.loaddatadeactivate(data);
        }

        [Route("getclasslistdeactivate")]
        public LP_OnlineExamDTO getclasslistdeactivate([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getclasslistdeactivate(data);
        }

        [Route("getsubjectlistdeactivate")]
        public LP_OnlineExamDTO getsubjectlistdeactivate([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getsubjectlistdeactivate(data);
        }

        [Route("GetQuestionList")]
        public LP_OnlineExamDTO GetQuestionList([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetQuestionList(data);
        }

        [Route("SaveDeactiveQuestionDetails")]
        public LP_OnlineExamDTO SaveDeactiveQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.SaveDeactiveQuestionDetails(data);
        }

        // Master Complexities

        [Route("getmastercompliexities/{id:int}")]
        public LP_OnlineExamDTO getmastercompliexities(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.getmastercompliexities(data);
        }

        [Route("SaveMasterComplexity")]
        public LP_OnlineExamDTO SaveMasterComplexity([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.SaveMasterComplexity(data);
        }

        [Route("DeactivateActivateComplexities")]
        public LP_OnlineExamDTO DeactivateActivateComplexities([FromBody] LP_OnlineExamDTO data)
        {
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.DeactivateActivateComplexities(data);
        }


        // Report
        [Route("LoadReport/{id:int}")]
        public LP_OnlineExamDTO LoadReport(int id)
        {
            LP_OnlineExamDTO data = new LP_OnlineExamDTO();
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.LoadReport(data);
        }

        [Route("GetReport")]
        public LP_OnlineExamDTO GetReport([FromBody]  LP_OnlineExamDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetReport(data);
        }

        [Route("GetStaffWiseExamReport")]
        public LP_OnlineExamDTO GetStaffWiseExamReport([FromBody]  LP_OnlineExamDTO data)
        {           
            data.MI_Id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.Userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.roleid = Convert.ToInt32(HttpContext.Session.GetInt32("RoleId"));
            data.ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            return _delg.GetStaffWiseExamReport(data);
        }
        

    }
}