using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

namespace IVRMUX.Delegates.NAAC.LP_OnlineExam
{
    public class LP_OnlineExamDelegate
    {
        CommonDelegate<LP_OnlineExamDTO, LP_OnlineExamDTO> _comm = new CommonDelegate<LP_OnlineExamDTO, LP_OnlineExamDTO>();

        // LP ONLINE EXAM CONFIG
        public LP_OnlineExamDTO getconfigloaddata(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getconfigloaddata");
        }
        public LP_OnlineExamDTO saveconfigdata(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/saveconfigdata");
        }

        // LP SCHOOL ONLINE MASTER QUESTION 
        public LP_OnlineExamDTO getmasterquestionloaddata(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getmasterquestionloaddata");
        }
        public LP_OnlineExamDTO getclasslist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getclasslist");
        }
        public LP_OnlineExamDTO getsubjectlist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getsubjectlist");
        }
        public LP_OnlineExamDTO gettopiclist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/gettopiclist");
        }
        public LP_OnlineExamDTO SaveMasterQuestionDetails(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SaveMasterQuestionDetails");
        }
        public LP_OnlineExamDTO EditMasterQuestion(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/EditMasterQuestion");
        }
        public LP_OnlineExamDTO ViewMasterQuesDoc(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewMasterQuesDoc");
        }
        public LP_OnlineExamDTO DeactivateActivateQuestion(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateQuestion");
        }
        public LP_OnlineExamDTO DeactivateActivateDocument(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateDocument");
        }
        public LP_OnlineExamDTO ViewMasterQuesOptions(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewMasterQuesOptions");
        }
        public LP_OnlineExamDTO ViewUploadOptionFiles(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewUploadOptionFiles");
        }
        public LP_OnlineExamDTO DeactivateActivateQuesOption(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateQuesOption");
        }
        public LP_OnlineExamDTO DeactivateActivateOptionsDocument(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateOptionsDocument");
        }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM  *****************//
        public LP_OnlineExamDTO getexammasterload(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getexammasterload");
        }
        public LP_OnlineExamDTO getexamclasslist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getexamclasslist");
        }
        public LP_OnlineExamDTO getexamsectionslist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getexamsectionslist");
        }
        public LP_OnlineExamDTO getexamsubjectlist(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getexamsubjectlist");
        }
        public LP_OnlineExamDTO GetSearchTopics(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/GetSearchTopics");
        }
        public LP_OnlineExamDTO SearchQuestions(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SearchQuestions");
        }
        public LP_OnlineExamDTO SaveMasterExamQuestionDetails(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SaveMasterExamQuestionDetails");
        }
        public LP_OnlineExamDTO EditMasterExamQuestion(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/EditMasterExamQuestion");
        }
        public LP_OnlineExamDTO ViewMasterExamQuesOptions(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewMasterExamQuesOptions");
        }
        public LP_OnlineExamDTO ViewMasterExamLevelDetails(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewMasterExamLevelDetails");
        }
        public LP_OnlineExamDTO ViewSavedLevelQuestons(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewSavedLevelQuestons");
        }
        public LP_OnlineExamDTO ViewMasterQuestionExamTopic(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewMasterQuestionExamTopic");
        }
        public LP_OnlineExamDTO ViewQuestionPaper(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/ViewQuestionPaper");
        }
        public LP_OnlineExamDTO DeactivateActivateMasterExam(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateMasterExam");
        }
        public LP_OnlineExamDTO DeactivateActivateExamQues(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateExamQues");
        }
        public LP_OnlineExamDTO DeactivateActivateExamQuesTopic(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateExamQuesTopic");
        }
        public LP_OnlineExamDTO SearchQuestionfilter(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SearchQuestionfilter");
        }
        public LP_OnlineExamDTO OnChangeMasterExam(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/OnChangeMasterExam");
        }
        public LP_OnlineExamDTO SaveLevelQuestionOrder(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SaveLevelQuestionOrder");
        }

        //Question Deactivate All
        public LP_OnlineExamDTO loaddatadeactivate(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/loaddatadeactivate");
        }
        public LP_OnlineExamDTO getclasslistdeactivate(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getclasslistdeactivate");
        }
        public LP_OnlineExamDTO getsubjectlistdeactivate(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getsubjectlistdeactivate");
        }
        public LP_OnlineExamDTO GetQuestionList(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/GetQuestionList");
        }
        public LP_OnlineExamDTO SaveDeactiveQuestionDetails(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SaveDeactiveQuestionDetails");
        }

        // Master Complexities
        public LP_OnlineExamDTO getmastercompliexities(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/getmastercompliexities");
        }
        public LP_OnlineExamDTO SaveMasterComplexity(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/SaveMasterComplexity");
        }
        public LP_OnlineExamDTO DeactivateActivateComplexities(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/DeactivateActivateComplexities");
        }

        // Report
        public LP_OnlineExamDTO LoadReport(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/LoadReport");
        }
        public LP_OnlineExamDTO GetReport(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/GetReport");
        }
        public LP_OnlineExamDTO GetStaffWiseExamReport(LP_OnlineExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineExamFacade/GetStaffWiseExamReport");
        }


    }
}
