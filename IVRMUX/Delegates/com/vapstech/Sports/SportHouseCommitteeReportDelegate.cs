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
    public class SportHouseCommitteeReportDelegate : Controller
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<House_Committe_Report_DTO, House_Committe_Report_DTO> COMMM = new CommonDelegate<House_Committe_Report_DTO, House_Committe_Report_DTO>();


        public House_Committe_Report_DTO Getdetails(House_Committe_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseCommitteeReportFacade/Getdetails/");

        }

        public House_Committe_Report_DTO showdetails(House_Committe_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseCommitteeReportFacade/showdetails/");

        }

        public House_Committe_Report_DTO get_House(House_Committe_Report_DTO data)
        {
            return COMMM.POSTDataSports(data, "SportHouseCommitteeReportFacade/get_House/");

        }
        
    }
}
