﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Exam;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Exam
{
    [Route("api/[controller]")]
    public class VikasaSchoolExamWiseCumulativeReportDelegates : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO> COMMM = new CommonDelegate<VikasaSubjectwiseCumulativeReportDTO, VikasaSubjectwiseCumulativeReportDTO>();


        public VikasaSubjectwiseCumulativeReportDTO Getdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/Getdetails/");
           
        }


        public VikasaSubjectwiseCumulativeReportDTO showdetails(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/showdetails/");
           
        }


        public VikasaSubjectwiseCumulativeReportDTO get_class(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/get_class/");

           
        }

        public VikasaSubjectwiseCumulativeReportDTO get_section(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/get_section/");

            
        }


        public VikasaSubjectwiseCumulativeReportDTO get_subject(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/get_subject/");
        
        }

        public VikasaSubjectwiseCumulativeReportDTO get_Exam(VikasaSubjectwiseCumulativeReportDTO data)
        {
            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/get_Exam/");
            
        }

        public VikasaSubjectwiseCumulativeReportDTO savedetails(VikasaSubjectwiseCumulativeReportDTO data)//Int32 IVRMM_Id        
        {

            return COMMM.POSTDataExam(data, "VikasaSchoolExamWiseCumulativeReportFacade/savedetails/");          
            
        }

    }
}
