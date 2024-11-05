
using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    public class MarksEntryDelegates
    {
        CommonDelegate<ExamMarksDTO, ExamMarksDTO> COMMM = new CommonDelegate<ExamMarksDTO, ExamMarksDTO>(); 
        public ExamMarksDTO getdetails(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/Getdetails/");
        }
        public ExamMarksDTO onselectAcdYear(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectAcdYear/");
        }
        public ExamMarksDTO onselectclass(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectclass/");
        }
        public ExamMarksDTO onselectSection(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectSection/");
        }
        public ExamMarksDTO onselectExam(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectExam/");
        }
        public ExamMarksDTO onselectSubject(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onselectSubject/");
        }
        public ExamMarksDTO onchangesubsubject(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onchangesubsubject/");
        }
        public ExamMarksDTO onsearch(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/onsearch/");
        }
        public ExamMarksDTO SaveMarks(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/SaveMarks/");
        }
        public ExamMarksDTO DeleteMarks(ExamMarksDTO id)
        {
            return COMMM.POSTDataExam(id, "MarksEntryFacade/DeleteMarks/");
        }
    }
}
