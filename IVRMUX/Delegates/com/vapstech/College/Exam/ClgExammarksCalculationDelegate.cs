using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExammarksCalculationDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgMarksCalculationsDTO, ClgMarksCalculationsDTO> COMMM = new CommonDelegate<ClgMarksCalculationsDTO, ClgMarksCalculationsDTO>();

        public ClgMarksCalculationsDTO Getdetails(ClgMarksCalculationsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExammarksCalculationFacade/Getdetails/");
        }
        public ClgMarksCalculationsDTO Calculation(ClgMarksCalculationsDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExammarksCalculationFacade/Calculation/");
        }
    }
}
