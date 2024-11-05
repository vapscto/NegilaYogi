using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgCumulativeReportDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgCumulativeReportDTO, ClgCumulativeReportDTO> COMMM = new CommonDelegate<ClgCumulativeReportDTO, ClgCumulativeReportDTO>();
        public ClgCumulativeReportDTO Getdetails(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/Getdetails/");
        }
        public ClgCumulativeReportDTO onchangeyear(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangeyear/");
        }
        public ClgCumulativeReportDTO onchangecourse(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangecourse/");
        }
        public ClgCumulativeReportDTO onchangebranch(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangebranch/");
        }
        public ClgCumulativeReportDTO onchangesemester(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangesemester/");
        }
        public ClgCumulativeReportDTO onchangesubjectscheme(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangesubjectscheme/");
        }
        public ClgCumulativeReportDTO onchangeschemetype(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/onchangeschemetype/");
        }
        public ClgCumulativeReportDTO Getcmreport(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/Getcmreport/");
        }
        public ClgCumulativeReportDTO GetCumulativeReportFormat2(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/GetCumulativeReportFormat2/");
        }
        public ClgCumulativeReportDTO GetProgresscardReport(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/GetProgresscardReport/");
        }
        public ClgCumulativeReportDTO GetJNUProgressCardReport1(ClgCumulativeReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgCumulativeReportFacade/GetJNUProgressCardReport1/");
        }
    }
}
