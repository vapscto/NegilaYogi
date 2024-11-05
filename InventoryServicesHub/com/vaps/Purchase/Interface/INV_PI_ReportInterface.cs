using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PI_ReportInterface
    {
       Task<INV_PurchaseIndentDTO> getloaddata(INV_PurchaseIndentDTO data);
        Task<INV_PurchaseIndentDTO> onreport(INV_PurchaseIndentDTO data);


    }


}
