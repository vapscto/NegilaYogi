using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Sports
{
    public class HouseInchargeReportDelegate
    {
        CommonDelegate<HouseInchargeReport_DTO, HouseInchargeReport_DTO> COMVISITOR = new CommonDelegate<HouseInchargeReport_DTO, HouseInchargeReport_DTO>();

        public HouseInchargeReport_DTO get_details(HouseInchargeReport_DTO obj)
        {
            return COMVISITOR.POSTDataSports(obj, "HouseInchargeReportFacade/get_details/");
        }
        public HouseInchargeReport_DTO get_house(HouseInchargeReport_DTO obj)
        {
            return COMVISITOR.POSTDataSports(obj, "HouseInchargeReportFacade/get_house/");
        }
        public HouseInchargeReport_DTO get_reports(HouseInchargeReport_DTO obj)
        {
            return COMVISITOR.POSTDataSports(obj, "HouseInchargeReportFacade/get_reports/");
        }

    }
}
