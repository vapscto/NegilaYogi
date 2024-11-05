using PreadmissionDTOs.NAAC.LP_OnlineExam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.LP_OnlineExam.Interface
{
    public interface LP_OnlineStudentExamInterface
    {
        LP_OnlineStudentExamDTO getloaddata(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO onselectsubject(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO getQuestion(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO Saveanswer(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO savedanswers(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO submitexam(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO GetViewMarks(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO SaveAnswerSheet(LP_OnlineStudentExamDTO data);

        // Report
        LP_OnlineStudentExamDTO getreport(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO onchangeyear(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO onchangeclass(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO OnchangeSection(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO onchangesubject(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO onchangesubject_studentmarks(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO OnChangeExam(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO getloaddatareport(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO ViewStudentWiseMarks(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO GetSearchDetails(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO SaveMarks(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO SaveStudentAnswerFileByStaff(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO ViewQuestion(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO SaveSubjectiveMarks(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO PublishToStudent(LP_OnlineStudentExamDTO data);

        // Pushing Online Exam Marks To Master Exam Marks
        LP_OnlineStudentExamDTO GetStudentListForPublish(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO CheckStudentMarksEntered(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO GetExam_OE_StudentList(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO SaveOE_Marks_ME(LP_OnlineStudentExamDTO data);

        //online exam not submitted list report
        LP_OnlineStudentExamDTO getNonSubmittedreport(LP_OnlineStudentExamDTO data);
        LP_OnlineStudentExamDTO MergeFiles(LP_OnlineStudentExamDTO data);
    }
}
