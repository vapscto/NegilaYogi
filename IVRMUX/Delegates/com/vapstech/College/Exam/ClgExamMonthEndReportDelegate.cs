using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Exam
{
    public class ClgExamMonthEndReportDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgExamMonthEndReportDTO, ClgExamMonthEndReportDTO> COMMM = new CommonDelegate<ClgExamMonthEndReportDTO, ClgExamMonthEndReportDTO>();
        public ClgExamMonthEndReportDTO getdetails(ClgExamMonthEndReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMonthEndReportFacade/getdetails/");
        }
        public ClgExamMonthEndReportDTO onreport(ClgExamMonthEndReportDTO data)
        {
            return COMMM.POSTcolExam(data, "ClgExamMonthEndReportFacade/onreport/");
        }
    }
}
