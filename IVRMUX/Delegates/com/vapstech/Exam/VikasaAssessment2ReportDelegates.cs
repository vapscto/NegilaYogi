﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaAssessment2ReportDelegates : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO> COMMM = new CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO>();



        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/Getdetails/");
            
        }

        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)//Int32 IVRMM_Id
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/savedetails/");

        }

        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/get_class/");
        }
        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/get_section/");
        }
      
        public VikasaSubjectwiseCumulativeReportDTO get_exam(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/get_exam/");
        }
        public VikasaSubjectwiseCumulativeReportDTO getcategory(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaAssessment2ReportFacade/getcategory/");
        }
    }
}
