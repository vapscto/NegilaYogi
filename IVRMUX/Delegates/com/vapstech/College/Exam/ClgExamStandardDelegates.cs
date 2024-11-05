using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExamStandardDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamStandardDTO, ExamStandardDTO> COMMM = new CommonDelegate<ExamStandardDTO, ExamStandardDTO>();

        public ExamStandardDTO Getdetails(ExamStandardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmStandardFacade/Getdetails/");
        }
        public ExamStandardDTO savedetails(ExamStandardDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgmStandardFacade/savedetails/");
        }
    }
}
