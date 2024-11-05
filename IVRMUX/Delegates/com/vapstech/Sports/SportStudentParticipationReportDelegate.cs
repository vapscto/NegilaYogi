using System;
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
    public class SportStudentParticipationReportDelegate : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentAgeCalcReport_DTO, StudentAgeCalcReport_DTO> COMMM = new CommonDelegate<StudentAgeCalcReport_DTO, StudentAgeCalcReport_DTO>();

        public StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/Getdetails/");
        }

        public StudentAgeCalcReport_DTO showdetails(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/showdetails/");
        }

        public StudentAgeCalcReport_DTO get_class(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_class/");
        }

        public StudentAgeCalcReport_DTO get_section(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_section/");
        }

        public StudentAgeCalcReport_DTO get_student(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_student/");
        }


        #region
        //public SportStudentParticipationReportDTO Getdetails(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/Getdetails/");

        //}
        //public SportStudentParticipationReportDTO showdetails(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/showdetails/");

        //}
        //public SportStudentParticipationReportDTO getevent(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/getevent/");

        //}
        //public SportStudentParticipationReportDTO get_class(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_class/");


        //}
        //public SportStudentParticipationReportDTO get_section(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_section/");


        //}
        //public SportStudentParticipationReportDTO get_student(SportStudentParticipationReportDTO data)
        //{
        //    return COMMM.POSTDataSports(data, "SportStudentParticipationReportFacade/get_student/");
        //}
        #endregion

    }
}
