using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PurchaseIndentInterface
    {
        INV_PurchaseIndentDTO getloaddata(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO savedetails(INV_PurchaseIndentDTO data);

        Task<INV_PurchaseIndentDTO> getpidetails(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO getprDetail(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO deactive(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO get_details(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO edit(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO genrateReceipt(INV_PurchaseIndentDTO data);
        INV_PurchaseIndentDTO deactiveM(INV_PurchaseIndentDTO data);

    }


}
