using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class SportsReportTeamPageDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SportsReportTeamPageDto, SportsReportTeamPageDto> COMMM = new CommonDelegate<SportsReportTeamPageDto, SportsReportTeamPageDto>();


        public SportsReportTeamPageDto saveRecord(SportsReportTeamPageDto data)
        {
            return COMMM.POSTDataSports(data, "SportsReportTeamPageFacade/saveRecord/");
        }
        public SportsReportTeamPageDto Getdetails(SportsReportTeamPageDto data)
        {
            return COMMM.POSTDataSports(data, "SportsReportTeamPageFacade/Getdetails/");
        }

        public SportsReportTeamPageDto showdetails(SportsReportTeamPageDto data)
        {
            return COMMM.POSTDataSports(data, "SportsReportTeamPageFacade/showdetails/");
        }


        public SportsReportTeamPageDto get_modeldata(SportsReportTeamPageDto obj)
        {
            return COMMM.POSTDataSports(obj, "SportsReportTeamPageFacade/get_modeldata/");
        }
        public SportsReportTeamPageDto get_student(SportsReportTeamPageDto data)
        {
            return COMMM.POSTDataSports(data, "SportsReportTeamPageFacade/get_student/");
        }

        public SportsReportTeamPageDto EditRecord(SportsReportTeamPageDto dTO)
        {
            return COMMM.POSTDataSports(dTO, "SportsReportTeamPageFacade/EditRecord/");
        }
        public SportsReportTeamPageDto deactivate(SportsReportTeamPageDto obj)
        {
            return COMMM.POSTDataSports(obj, "SportsReportTeamPageFacade/deactivate/");
        }

        public SportsReportTeamPageDto SaveRecords(SportsReportTeamPageDto data)
        {
            return COMMM.POSTDataSports(data, "SportsReportTeamPageFacade/SaveRecords/");
        }

        public SportsReportTeamPageDto GetEditData(SportsReportTeamPageDto dTO)
        {
            return COMMM.POSTDataSports(dTO, "SportsReportTeamPageFacade/GetEditData/");
        }

        public SportsReportTeamPageDto deactivated(SportsReportTeamPageDto obj)
        {
            return COMMM.POSTDataSports(obj, "SportsReportTeamPageFacade/deactivated/");
        }
    }
}
