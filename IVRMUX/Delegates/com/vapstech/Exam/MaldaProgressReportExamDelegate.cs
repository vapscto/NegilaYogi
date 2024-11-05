using CommonLibrary;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Exam
{
    public class MaldaProgressReportExamDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MaldaProgressReportExam_DTO, MaldaProgressReportExam_DTO> COMMM = new CommonDelegate<MaldaProgressReportExam_DTO, MaldaProgressReportExam_DTO>();
        public MaldaProgressReportExam_DTO Getdetails(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/Getdetails/");

        }
        public MaldaProgressReportExam_DTO showdetails(MaldaProgressReportExam_DTO data)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/savedetails/");

        }
        public MaldaProgressReportExam_DTO onchangeyear(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/onchangeyear/");
        }
        public MaldaProgressReportExam_DTO onchangeclass(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/onchangeclass/");
        }

        public MaldaProgressReportExam_DTO onchangesection(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/onchangesection/");
        }
        public MaldaProgressReportExam_DTO getreportpromotion(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/getreportpromotion/");
        }
        public MaldaProgressReportExam_DTO ixpromotionreport(MaldaProgressReportExam_DTO data)
        {
            return COMMM.POSTDataExam(data, "MaldaProgressReportExamFacade/ixpromotionreport/");
        }


    }
}
