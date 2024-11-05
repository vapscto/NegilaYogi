using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_MonthEndReportInterface
    {
        INV_MonthEndReportDTO getloaddata(INV_MonthEndReportDTO data);
        Task<INV_MonthEndReportDTO> getmonthreport(INV_MonthEndReportDTO data);

    }


}
