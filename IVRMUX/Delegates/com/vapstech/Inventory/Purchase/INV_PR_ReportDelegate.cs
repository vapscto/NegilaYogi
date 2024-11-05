using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PR_ReportDelegate
    {
        CommonDelegate<INV_PurchaseRequisitionDTO, INV_PurchaseRequisitionDTO> COMINV = new CommonDelegate<INV_PurchaseRequisitionDTO, INV_PurchaseRequisitionDTO>();
        public INV_PurchaseRequisitionDTO getloaddata(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PR_ReportFacade/getloaddata/");
        }
        public INV_PurchaseRequisitionDTO onreport(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PR_ReportFacade/onreport/");
        }

        
    }
}
