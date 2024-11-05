using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_ItemConsumptionReportInterface
    {
       Task<INV_ItemConsumptionDTO> getloaddata(INV_ItemConsumptionDTO data);
        Task<INV_ItemConsumptionDTO> onreport(INV_ItemConsumptionDTO data);


    }


}
