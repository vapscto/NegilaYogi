using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class CollegeExamGeneralReportDelegate
    {
        CommonDelegate<CollegeExamGeneralReportDTO, CollegeExamGeneralReportDTO> _com = new CommonDelegate<CollegeExamGeneralReportDTO, CollegeExamGeneralReportDTO>();

        public CollegeExamGeneralReportDTO MasterGradeReportLoadData (CollegeExamGeneralReportDTO data)
        {
            return _com.POSTcolExam(data, "CollegeExamGeneralReportFacade/MasterGradeReportLoadData");
        }
        public CollegeExamGeneralReportDTO MasterGradeReportDetails(CollegeExamGeneralReportDTO data)
        {
            return _com.POSTcolExam(data, "CollegeExamGeneralReportFacade/MasterGradeReportDetails");
        }
    }
}
