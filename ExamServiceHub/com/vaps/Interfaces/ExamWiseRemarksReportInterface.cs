using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamWiseRemarksReportInterface
    {
        ExamWiseRemarksReportDTO LoadData(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO OnChangeYear(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO OnChangeClass(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO OnChangeSection(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO OnChangeExam(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO GetExamWiseRemarksReport(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO GetExamSubjectWiseRemarks_PTReport(ExamWiseRemarksReportDTO data);
        ExamWiseRemarksReportDTO GetTermWiseRemarksReport(ExamWiseRemarksReportDTO data);
    }
}
