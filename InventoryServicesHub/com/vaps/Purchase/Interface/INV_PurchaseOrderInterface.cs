using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PurchaseOrderInterface
    {
        INV_PurchaseOrderDTO getloaddata(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO getqtDetail(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO getitemtax(INV_PurchaseOrderDTO data);        
        INV_PurchaseOrderDTO savedetails(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO deactiveM(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO deactive(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO deactiveTx(INV_PurchaseOrderDTO data);
        INV_PurchaseOrderDTO get_modeldetails(INV_PurchaseOrderDTO data);
        

    }


}
