using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class StudentAgeCalcReportDelegate
    {

        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<StudentAgeCalcReport_DTO, StudentAgeCalcReport_DTO> COMMM = new CommonDelegate<StudentAgeCalcReport_DTO, StudentAgeCalcReport_DTO>();


        public StudentAgeCalcReport_DTO Getdetails(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "StudentAgeCalcReportFacade/Getdetails/");
        }

        public StudentAgeCalcReport_DTO showdetails(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "StudentAgeCalcReportFacade/showdetails/");
        }

        public StudentAgeCalcReport_DTO get_class(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "StudentAgeCalcReportFacade/get_class/");
        }

        public StudentAgeCalcReport_DTO get_section(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "StudentAgeCalcReportFacade/get_section/");
        }

        public StudentAgeCalcReport_DTO get_student(StudentAgeCalcReport_DTO data)
        {
            return COMMM.POSTDataSports(data, "StudentAgeCalcReportFacade/get_student/");
        }

    }
}
