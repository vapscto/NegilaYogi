using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PO_ReportDelegate
    {
        CommonDelegate<INV_PurchaseOrderDTO, INV_PurchaseOrderDTO> COMINV = new CommonDelegate<INV_PurchaseOrderDTO, INV_PurchaseOrderDTO>();
        public INV_PurchaseOrderDTO getloaddata(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ReportFacade/getloaddata/");
        }
        public INV_PurchaseOrderDTO onreport(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PO_ReportFacade/onreport/");
        }

        
    }
}
