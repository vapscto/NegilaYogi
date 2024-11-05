using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MonthEndReportDelegate
    {
        CommonDelegate<INV_MonthEndReportDTO, INV_MonthEndReportDTO> COMINV = new CommonDelegate<INV_MonthEndReportDTO, INV_MonthEndReportDTO>();
        public INV_MonthEndReportDTO getloaddata(INV_MonthEndReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MonthEndReportFacade/getloaddata/");
        }

        public INV_MonthEndReportDTO getmonthreport(INV_MonthEndReportDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MonthEndReportFacade/getmonthreport/");
        }
     

    }
}
