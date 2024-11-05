using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PI_ReportDelegate
    {
        CommonDelegate<INV_PurchaseIndentDTO, INV_PurchaseIndentDTO> COMINV = new CommonDelegate<INV_PurchaseIndentDTO, INV_PurchaseIndentDTO>();
        public INV_PurchaseIndentDTO getloaddata(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ReportFacade/getloaddata/");
        }
        public INV_PurchaseIndentDTO onreport(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PI_ReportFacade/onreport/");
        }

        
    }
}
