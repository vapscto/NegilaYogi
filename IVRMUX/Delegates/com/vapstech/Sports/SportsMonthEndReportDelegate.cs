using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class SportsMonthEndReportDelegate
    {
        CommonDelegate<SportsMonthEndReport_DTO, SportsMonthEndReport_DTO> COMSPRT = new CommonDelegate<SportsMonthEndReport_DTO, SportsMonthEndReport_DTO>();

        public SportsMonthEndReport_DTO getdeatils(SportsMonthEndReport_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsMonthEndReportFacade/getdeatils/");
        }
        public SportsMonthEndReport_DTO GetReport(SportsMonthEndReport_DTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "SportsMonthEndReportFacade/GetReport/");
        }

    }
}
