using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

namespace IVRMUX.Delegates.NAAC.LP_OnlineExam
{
    public class LP_OnlineStudentExamDelegate
    {
        CommonDelegate<LP_OnlineStudentExamDTO, LP_OnlineStudentExamDTO> _comm = new CommonDelegate<LP_OnlineStudentExamDTO, LP_OnlineStudentExamDTO>();

        public LP_OnlineStudentExamDTO getloaddata(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/getloaddata");
        }
        public LP_OnlineStudentExamDTO onselectsubject(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/onselectsubject");
        }
        public LP_OnlineStudentExamDTO getQuestion(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/getQuestion");
        }
        public LP_OnlineStudentExamDTO Saveanswer(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/Saveanswer");
        }
        public LP_OnlineStudentExamDTO savedanswers(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/savedanswers");
        }
        public LP_OnlineStudentExamDTO submitexam(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/submitexam");
        }
        public LP_OnlineStudentExamDTO GetViewMarks(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/GetViewMarks");
        }
        public LP_OnlineStudentExamDTO SaveAnswerSheet(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/SaveAnswerSheet");
        }

        // Report
        public LP_OnlineStudentExamDTO getloaddatareport(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/getloaddatareport");
        }
        public LP_OnlineStudentExamDTO onchangeyear(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/onchangeyear");
        }
        public LP_OnlineStudentExamDTO onchangeclass(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/onchangeclass");
        }
        public LP_OnlineStudentExamDTO OnchangeSection(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/OnchangeSection");
        }
        public LP_OnlineStudentExamDTO onchangesubject(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/onchangesubject");
        }
        public LP_OnlineStudentExamDTO onchangesubject_studentmarks(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/onchangesubject_studentmarks");
        }
        public LP_OnlineStudentExamDTO OnChangeExam(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/OnChangeExam");
        }
        public LP_OnlineStudentExamDTO getreport(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/getreport");
        }
        public LP_OnlineStudentExamDTO ViewStudentWiseMarks(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/ViewStudentWiseMarks");
        }
        public LP_OnlineStudentExamDTO GetSearchDetails(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/GetSearchDetails");
        }
        public LP_OnlineStudentExamDTO SaveMarks(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/SaveMarks");
        }
        public LP_OnlineStudentExamDTO SaveStudentAnswerFileByStaff(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/SaveStudentAnswerFileByStaff");
        }
        public LP_OnlineStudentExamDTO ViewQuestion(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/ViewQuestion");
        }
        public LP_OnlineStudentExamDTO SaveSubjectiveMarks(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/SaveSubjectiveMarks");
        }
        public LP_OnlineStudentExamDTO PublishToStudent(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/PublishToStudent");
        }

        // Pushing Online Exam Marks To Master Exam Marks
        public LP_OnlineStudentExamDTO GetStudentListForPublish(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/GetStudentListForPublish");
        }
        public LP_OnlineStudentExamDTO CheckStudentMarksEntered(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/CheckStudentMarksEntered");
        }
        public LP_OnlineStudentExamDTO GetExam_OE_StudentList(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/GetExam_OE_StudentList");
        }
        public LP_OnlineStudentExamDTO SaveOE_Marks_ME(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/SaveOE_Marks_ME");
        }

        //online exam not submitted list report
        public LP_OnlineStudentExamDTO getNonSubmittedreport(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/getNonSubmittedreport");
        }
        public LP_OnlineStudentExamDTO MergeFiles(LP_OnlineStudentExamDTO data)
        {
            return _comm.naacdetailsbypost(data, "LP_OnlineStudentExamFacade/MergeFiles");
        }
    }
}
