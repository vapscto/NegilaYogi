﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Sports;
using CommonLibrary;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    [Route("api/[controller]")]
    public class SportTopperListReportDelegate : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SportStudentParticipationReportDTO, SportStudentParticipationReportDTO> COMMM = new CommonDelegate<SportStudentParticipationReportDTO, SportStudentParticipationReportDTO>();


        public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SportTopperListReportFacade/Getdetails/");

        }


        public SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SportTopperListReportFacade/showdetails/");

        }


        public SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SportTopperListReportFacade/get_class/");


        }

        public SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SportTopperListReportFacade/get_section/");


        }

        public SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO data)
        {
            return COMMM.POSTDataSports(data, "SportTopperListReportFacade/get_student/");
        }




    }
}
