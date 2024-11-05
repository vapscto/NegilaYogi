using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PurchaseRequisitionInterface
    {
        INV_PurchaseRequisitionDTO getloaddata(INV_PurchaseRequisitionDTO data);
        INV_PurchaseRequisitionDTO get_prdetails(INV_PurchaseRequisitionDTO data);
        
        INV_PurchaseRequisitionDTO getitemDetail(INV_PurchaseRequisitionDTO data);
        INV_PurchaseRequisitionDTO savedetails(INV_PurchaseRequisitionDTO data);
        INV_PurchaseRequisitionDTO edit(INV_PurchaseRequisitionDTO data);
        INV_PurchaseRequisitionDTO deactiveM(INV_PurchaseRequisitionDTO data);
        INV_PurchaseRequisitionDTO deactive(INV_PurchaseRequisitionDTO data);
     
    }


}
