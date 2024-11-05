using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PurchaseIndentDelegate
    {
        CommonDelegate<INV_PurchaseIndentDTO, INV_PurchaseIndentDTO> COMINV = new CommonDelegate<INV_PurchaseIndentDTO, INV_PurchaseIndentDTO>();
        public INV_PurchaseIndentDTO getloaddata(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/getloaddata/");
        }

        
        public INV_PurchaseIndentDTO getpidetails(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/getpidetails/");
        }
        public INV_PurchaseIndentDTO savedetails(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/savedetails/");
        }
        public INV_PurchaseIndentDTO getprDetail(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/getprDetail/");
        }
        public INV_PurchaseIndentDTO deactiveM(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/deactiveM/");
        }
        public INV_PurchaseIndentDTO deactive(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/deactive/");
        }
        public INV_PurchaseIndentDTO get_details(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/get_details/");
        }
        public INV_PurchaseIndentDTO edit(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/edit/");
        }
        public INV_PurchaseIndentDTO genrateReceipt(INV_PurchaseIndentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseIndentFacade/genrateReceipt/");
        }

        

    }
}
