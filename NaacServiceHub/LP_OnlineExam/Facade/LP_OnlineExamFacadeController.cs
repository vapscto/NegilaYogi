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
    public class LP_OnlineExamFacadeController : Controller
    {
        public LP_OnlineExamInterface _interface;

        public LP_OnlineExamFacadeController(LP_OnlineExamInterface _inter)
        {
            _interface = _inter;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // LP ONLINE EXAM CONFIG

        [Route("getconfigloaddata")]
        public LP_OnlineExamDTO getconfigloaddata([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getconfigloaddata(data);
        }

        [Route("saveconfigdata")]
        public LP_OnlineExamDTO saveconfigdata([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.saveconfigdata(data);
        }

        // LP SCHOOL ONLINE MASTER QUESTION

        [Route("getmasterquestionloaddata")]
        public LP_OnlineExamDTO getmasterquestionloaddata([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getmasterquestionloaddata(data);
        }

        [Route("getclasslist")]
        public LP_OnlineExamDTO getclasslist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getclasslist(data);
        }

        [Route("getsubjectlist")]
        public LP_OnlineExamDTO getsubjectlist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getsubjectlist(data);
        }

        [Route("gettopiclist")]
        public LP_OnlineExamDTO gettopiclist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.gettopiclist(data);
        }

        [Route("SaveMasterQuestionDetails")]
        public LP_OnlineExamDTO SaveMasterQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SaveMasterQuestionDetails(data);
        }

        [Route("EditMasterQuestion")]
        public LP_OnlineExamDTO EditMasterQuestion([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.EditMasterQuestion(data);
        }

        [Route("ViewMasterQuesDoc")]
        public LP_OnlineExamDTO ViewMasterQuesDoc([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewMasterQuesDoc(data);
        }

        [Route("DeactivateActivateQuestion")]
        public LP_OnlineExamDTO DeactivateActivateQuestion([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateQuestion(data);
        }

        [Route("DeactivateActivateDocument")]
        public LP_OnlineExamDTO DeactivateActivateDocument([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateDocument(data);
        }

        [Route("ViewMasterQuesOptions")]
        public LP_OnlineExamDTO ViewMasterQuesOptions([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewMasterQuesOptions(data);
        }

        [Route("ViewUploadOptionFiles")]
        public LP_OnlineExamDTO ViewUploadOptionFiles([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewUploadOptionFiles(data);
        }

        [Route("DeactivateActivateQuesOption")]
        public LP_OnlineExamDTO DeactivateActivateQuesOption([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateQuesOption(data);
        }

        [Route("DeactivateActivateOptionsDocument")]
        public LP_OnlineExamDTO DeactivateActivateOptionsDocument([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateOptionsDocument(data);
        }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM *****************//

        [Route("getexammasterload")]
        public LP_OnlineExamDTO getexammasterload([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getexammasterload(data);
        }

        [Route("getexamclasslist")]
        public LP_OnlineExamDTO getexamclasslist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getexamclasslist(data);
        }

        [Route("getexamsectionslist")]
        public LP_OnlineExamDTO getexamsectionslist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getexamsectionslist(data);
        }

        [Route("getexamsubjectlist")]
        public LP_OnlineExamDTO getexamsubjectlist([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getexamsubjectlist(data);
        }

        [Route("SearchQuestions")]
        public LP_OnlineExamDTO SearchQuestions([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SearchQuestions(data);
        }

        [Route("GetSearchTopics")]
        public LP_OnlineExamDTO GetSearchTopics([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.GetSearchTopics(data);
        }

        [Route("SaveMasterExamQuestionDetails")]
        public LP_OnlineExamDTO SaveMasterExamQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SaveMasterExamQuestionDetails(data);
        }

        [Route("EditMasterExamQuestion")]
        public LP_OnlineExamDTO EditMasterExamQuestion([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.EditMasterExamQuestion(data);
        }

        [Route("ViewMasterExamQuesOptions")]
        public LP_OnlineExamDTO ViewMasterExamQuesOptions([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewMasterExamQuesOptions(data);
        }

        [Route("ViewSavedLevelQuestons")]
        public LP_OnlineExamDTO ViewSavedLevelQuestons([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewSavedLevelQuestons(data);
        }

        [Route("ViewMasterExamLevelDetails")]
        public LP_OnlineExamDTO ViewMasterExamLevelDetails([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewMasterExamLevelDetails(data);
        }

        [Route("ViewMasterQuestionExamTopic")]
        public LP_OnlineExamDTO ViewMasterQuestionExamTopic([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewMasterQuestionExamTopic(data);
        }

        [Route("ViewQuestionPaper")]
        public LP_OnlineExamDTO ViewQuestionPaper([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.ViewQuestionPaper(data);
        }

        [Route("DeactivateActivateMasterExam")]
        public LP_OnlineExamDTO DeactivateActivateMasterExam([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateMasterExam(data);
        }

        [Route("DeactivateActivateExamQues")]
        public LP_OnlineExamDTO DeactivateActivateExamQues([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateExamQues(data);
        }

        [Route("DeactivateActivateExamQuesTopic")]
        public LP_OnlineExamDTO DeactivateActivateExamQuesTopic([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateExamQuesTopic(data);
        }

        [Route("SearchQuestionfilter")]
        public LP_OnlineExamDTO SearchQuestionfilter([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SearchQuestionfilter(data);
        }

        [Route("OnChangeMasterExam")]
        public LP_OnlineExamDTO OnChangeMasterExam([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.OnChangeMasterExam(data);
        }

        [Route("SaveLevelQuestionOrder")]
        public LP_OnlineExamDTO SaveLevelQuestionOrder([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SaveLevelQuestionOrder(data);
        }

        // Load  Question Deactivate All

        [Route("loaddatadeactivate")]
        public LP_OnlineExamDTO loaddatadeactivate([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.loaddatadeactivate(data);
        }

        [Route("getclasslistdeactivate")]
        public LP_OnlineExamDTO getclasslistdeactivate([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getclasslistdeactivate(data);
        }

        [Route("getsubjectlistdeactivate")]
        public LP_OnlineExamDTO getsubjectlistdeactivate([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getsubjectlistdeactivate(data);
        }

        [Route("GetQuestionList")]
        public LP_OnlineExamDTO GetQuestionList([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.GetQuestionList(data);
        }

        [Route("SaveDeactiveQuestionDetails")]
        public LP_OnlineExamDTO SaveDeactiveQuestionDetails([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SaveDeactiveQuestionDetails(data);
        }

        // Master Complexities

        [Route("getmastercompliexities")]
        public LP_OnlineExamDTO getmastercompliexities([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.getmastercompliexities(data);
        }

        [Route("SaveMasterComplexity")]
        public LP_OnlineExamDTO SaveMasterComplexity([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.SaveMasterComplexity(data);
        }

        [Route("DeactivateActivateComplexities")]
        public LP_OnlineExamDTO DeactivateActivateComplexities([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.DeactivateActivateComplexities(data);
        }

        // Report
        [Route("LoadReport")]
        public LP_OnlineExamDTO LoadReport([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.LoadReport(data);
        }

        [Route("GetReport")]
        public LP_OnlineExamDTO GetReport([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.GetReport(data);
        }

        [Route("GetStaffWiseExamReport")]
        public LP_OnlineExamDTO GetStaffWiseExamReport([FromBody] LP_OnlineExamDTO data)
        {
            return _interface.GetStaffWiseExamReport(data);
        }
    }
}
