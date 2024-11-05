using PreadmissionDTOs.NAAC.LP_OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.LP_OnlineExam.Interface
{
    public interface LP_OnlineExamInterface
    {
        // LP ONLINE EXAM CONFIG
        LP_OnlineExamDTO getconfigloaddata(LP_OnlineExamDTO data);
        LP_OnlineExamDTO saveconfigdata(LP_OnlineExamDTO data);

        // LP SCHOOL ONLINE MASTER QUESTION 
        LP_OnlineExamDTO getmasterquestionloaddata(LP_OnlineExamDTO data);
        LP_OnlineExamDTO getclasslist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO getsubjectlist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO gettopiclist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SaveMasterQuestionDetails(LP_OnlineExamDTO data);
        LP_OnlineExamDTO EditMasterQuestion(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewMasterQuesDoc(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateQuestion(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateDocument(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewMasterQuesOptions(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewUploadOptionFiles(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateQuesOption(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateOptionsDocument(LP_OnlineExamDTO data);

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM  *****************//
        LP_OnlineExamDTO getexammasterload(LP_OnlineExamDTO data);
        LP_OnlineExamDTO getexamclasslist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO getexamsectionslist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO getexamsubjectlist(LP_OnlineExamDTO data);
        LP_OnlineExamDTO GetSearchTopics(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SearchQuestions(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SaveMasterExamQuestionDetails(LP_OnlineExamDTO data);
        LP_OnlineExamDTO EditMasterExamQuestion(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewMasterExamQuesOptions(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewMasterExamLevelDetails(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewSavedLevelQuestons(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewMasterQuestionExamTopic(LP_OnlineExamDTO data);
        LP_OnlineExamDTO ViewQuestionPaper(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateMasterExam(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateExamQues(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateExamQuesTopic(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SearchQuestionfilter(LP_OnlineExamDTO data);
        LP_OnlineExamDTO OnChangeMasterExam(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SaveLevelQuestionOrder(LP_OnlineExamDTO data);

        // Master Question Deactivate All
        LP_OnlineExamDTO loaddatadeactivate( LP_OnlineExamDTO data);
        LP_OnlineExamDTO getclasslistdeactivate( LP_OnlineExamDTO data);
        LP_OnlineExamDTO getsubjectlistdeactivate( LP_OnlineExamDTO data);
        LP_OnlineExamDTO GetQuestionList( LP_OnlineExamDTO data);
        LP_OnlineExamDTO SaveDeactiveQuestionDetails( LP_OnlineExamDTO data);

        // Master Complexities
        LP_OnlineExamDTO getmastercompliexities(LP_OnlineExamDTO data);
        LP_OnlineExamDTO SaveMasterComplexity(LP_OnlineExamDTO data);
        LP_OnlineExamDTO DeactivateActivateComplexities(LP_OnlineExamDTO data);

        // Report
        LP_OnlineExamDTO LoadReport(LP_OnlineExamDTO data);
        LP_OnlineExamDTO GetReport(LP_OnlineExamDTO data);
        LP_OnlineExamDTO GetStaffWiseExamReport(LP_OnlineExamDTO data);
    }
}