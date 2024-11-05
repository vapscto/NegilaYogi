using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PR_ReportInterface
    {
       Task<INV_PurchaseRequisitionDTO> getloaddata(INV_PurchaseRequisitionDTO data);
        Task<INV_PurchaseRequisitionDTO> onreport(INV_PurchaseRequisitionDTO data);


    }


}
