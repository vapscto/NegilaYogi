using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgMarksEntryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgMarksEntryDTO, ClgMarksEntryDTO> COMMM = new CommonDelegate<ClgMarksEntryDTO, ClgMarksEntryDTO>();

        public ClgMarksEntryDTO getdetails(ClgMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgMarksEntryFacade/getdetails/");
        }
        public ClgMarksEntryDTO onchangeyear(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/onchangeyear/");
        }
        
        public ClgMarksEntryDTO onchangecourse(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/onchangecourse/");
        }
        public ClgMarksEntryDTO onchangebranch(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/onchangebranch/");
        }
        public ClgMarksEntryDTO get_exams(ClgMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgMarksEntryFacade/get_exams/");
        }
        public ClgMarksEntryDTO get_subjects(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/get_subjects/");
        }
        public ClgMarksEntryDTO getsubjectscheme(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/getsubjectscheme/");
        }
        public ClgMarksEntryDTO getsubjectschemetype(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/getsubjectschemetype/");
        }
        public ClgMarksEntryDTO onsearch(ClgMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgMarksEntryFacade/onsearch/");
        }
       
        public ClgMarksEntryDTO SaveMarks(ClgMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "ClgMarksEntryFacade/SaveMarks/");
        }
       
        
    }
}
