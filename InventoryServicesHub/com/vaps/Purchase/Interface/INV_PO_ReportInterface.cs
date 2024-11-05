using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PO_ReportInterface
    {
       Task<INV_PurchaseOrderDTO> getloaddata(INV_PurchaseOrderDTO data);
        Task<INV_PurchaseOrderDTO> onreport(INV_PurchaseOrderDTO data);


    }


}
