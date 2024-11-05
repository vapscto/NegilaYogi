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
    public class SportHouseReportDelegate : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<House_Report_DTO, House_Report_DTO> COMMM = new CommonDelegate<House_Report_DTO, House_Report_DTO>();


        public House_Report_DTO Getdetails(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/Getdetails/");
        }


        public House_Report_DTO showdetails(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/showdetails/");
        }
        //showdetailsNew
        public House_Report_DTO showdetailsNew(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/showdetailsNew/");
        }

        public House_Report_DTO get_class(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/get_class/");
        }

        public House_Report_DTO get_section(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/get_section/");
        }


        public House_Report_DTO get_student(House_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseReportFacade/get_student/");
        }
    }
}
