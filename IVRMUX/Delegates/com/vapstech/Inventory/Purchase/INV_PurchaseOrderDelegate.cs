using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PurchaseOrderDelegate
    {
        CommonDelegate<INV_PurchaseOrderDTO, INV_PurchaseOrderDTO> COMINV = new CommonDelegate<INV_PurchaseOrderDTO, INV_PurchaseOrderDTO>();
        public INV_PurchaseOrderDTO getloaddata(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/getloaddata/");
        }
        public INV_PurchaseOrderDTO getqtDetail(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/getqtDetail/");
        }
        public INV_PurchaseOrderDTO getitemtax(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/getitemtax/");
        }        
        public INV_PurchaseOrderDTO savedetails(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/savedetails/");
        }   
        public INV_PurchaseOrderDTO deactiveM(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/deactiveM/");
        }
        public INV_PurchaseOrderDTO deactive(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/deactive/");
        }
        public INV_PurchaseOrderDTO deactiveTx(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/deactiveTx/");
        }
        public INV_PurchaseOrderDTO get_modeldetails(INV_PurchaseOrderDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseOrderFacade/get_modeldetails/");
        }

        


    }
}
