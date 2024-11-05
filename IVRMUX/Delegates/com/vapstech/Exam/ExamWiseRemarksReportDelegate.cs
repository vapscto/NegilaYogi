using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamWiseRemarksReportDelegate
    {
        CommonDelegate<ExamWiseRemarksReportDTO, ExamWiseRemarksReportDTO> _comm = new CommonDelegate<ExamWiseRemarksReportDTO, ExamWiseRemarksReportDTO>();
        public ExamWiseRemarksReportDTO LoadData(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/LoadData");
        }     
        public ExamWiseRemarksReportDTO OnChangeYear(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/OnChangeYear");
        }
        public ExamWiseRemarksReportDTO OnChangeClass(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/OnChangeClass");
        }
        public ExamWiseRemarksReportDTO OnChangeSection(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/OnChangeSection");
        }
        public ExamWiseRemarksReportDTO OnChangeExam(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/OnChangeExam");
        }
        public ExamWiseRemarksReportDTO GetExamWiseRemarksReport(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/GetExamWiseRemarksReport");
        }
        public ExamWiseRemarksReportDTO GetExamSubjectWiseRemarks_PTReport(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/GetExamSubjectWiseRemarks_PTReport");
        }
        public ExamWiseRemarksReportDTO GetTermWiseRemarksReport(ExamWiseRemarksReportDTO data)
        {
            return _comm.POSTDataExam(data, "ExamWiseRemarksReportFacade/GetTermWiseRemarksReport");
        }
    }
}
