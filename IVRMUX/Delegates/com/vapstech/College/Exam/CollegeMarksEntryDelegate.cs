using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class CollegeMarksEntryDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<CollegeMarksEntryDTO, CollegeMarksEntryDTO> COMMM = new CommonDelegate<CollegeMarksEntryDTO, CollegeMarksEntryDTO>();

        public CollegeMarksEntryDTO getdetails(CollegeMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "CollegeMarksEntryFacade/getdetails/");
        }
        public CollegeMarksEntryDTO onchangeyear(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/onchangeyear/");
        }

        public CollegeMarksEntryDTO onchangecourse(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/onchangecourse/");
        }
        public CollegeMarksEntryDTO onchangebranch(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/onchangebranch/");
        }
        public CollegeMarksEntryDTO get_exams(CollegeMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "CollegeMarksEntryFacade/get_exams/");
        }
        public CollegeMarksEntryDTO get_subjects(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/get_subjects/");
        }
        public CollegeMarksEntryDTO getsubjectscheme(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/getsubjectscheme/");
        }
        public CollegeMarksEntryDTO getsubjectschemetype(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/getsubjectschemetype/");
        }
        public CollegeMarksEntryDTO onsearch(CollegeMarksEntryDTO data)
        {
            return COMMM.POSTcolExam(data, "CollegeMarksEntryFacade/onsearch/");
        }

        public CollegeMarksEntryDTO SaveMarks(CollegeMarksEntryDTO id)
        {
            return COMMM.POSTcolExam(id, "CollegeMarksEntryFacade/SaveMarks/");
        }

    }
}
