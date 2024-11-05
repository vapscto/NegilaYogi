using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class ExamWiseTermReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ExamWiseTermReport_DTO, ExamWiseTermReport_DTO> COMMM = new CommonDelegate<ExamWiseTermReport_DTO, ExamWiseTermReport_DTO>();


        public ExamWiseTermReport_DTO Getdetails(ExamWiseTermReport_DTO data)
        {
            return COMMM.POSTDataExam(data, "ExamWiseTermReportFacade/Getdetails/");
        }       
        public ExamWiseTermReport_DTO onchangeyear(ExamWiseTermReport_DTO data)
        {
            return COMMM.POSTDataExam(data, "ExamWiseTermReportFacade/onchangeyear/");
        }
        public ExamWiseTermReport_DTO onchangeclass(ExamWiseTermReport_DTO data)
        {
            return COMMM.POSTDataExam(data, "ExamWiseTermReportFacade/onchangeclass/");
        }

        public ExamWiseTermReport_DTO onchangesection(ExamWiseTermReport_DTO data)
        {
            return COMMM.POSTDataExam(data, "ExamWiseTermReportFacade/onchangesection/");
        }


    }
}
