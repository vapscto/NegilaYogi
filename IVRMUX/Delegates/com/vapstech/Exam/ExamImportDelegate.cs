using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;

namespace corewebapi18072016.Delegates.com.vaps.Exam
{
    public class ExamImportDelegate
    {

        CommonDelegate<ExamImportStudentDTO, ExamImportStudentDTO> COMMM1 = new CommonDelegate<ExamImportStudentDTO, ExamImportStudentDTO>();

        CommonDelegate<ExamMarksDTO, ExamMarksDTO> COMMM = new CommonDelegate<ExamMarksDTO, ExamMarksDTO>();

        public ExamImportStudentDTO ImportMarks(ExamImportStudentDTO data)
        {

            return COMMM1.POSTDataExam(data, "ExamImportFacade/ImportMarks/");
        }

        public ExamMarksDTO getdetails(ExamMarksDTO id)
        {

            return COMMM.POSTDataExam(id, "ExamImportFacade/Getdetails/");
           
        }

        public ExamMarksDTO onselectAcdYear(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "ExamImportFacade/onselectAcdYear/");
        }
        public ExamMarksDTO onselectclass(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "ExamImportFacade/onselectclass/");
        }
        public ExamMarksDTO onselectSection(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "ExamImportFacade/onselectSection/");
        }
        public ExamMarksDTO onselectExam(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "ExamImportFacade/onselectExam/");
        }
        public ExamMarksDTO onsearch(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "ExamImportFacade/onsearch/");
        }

    }
}
