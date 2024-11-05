using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_PurchaseRequisitionDelegate
    {
        CommonDelegate<INV_PurchaseRequisitionDTO, INV_PurchaseRequisitionDTO> COMINV = new CommonDelegate<INV_PurchaseRequisitionDTO, INV_PurchaseRequisitionDTO>();
        public INV_PurchaseRequisitionDTO getloaddata(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/getloaddata/");
        }

        public INV_PurchaseRequisitionDTO get_prdetails(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/get_prdetails/");
        }
        public INV_PurchaseRequisitionDTO getitemDetail(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/getitemDetail/");
        }
        public INV_PurchaseRequisitionDTO savedetails(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/savedetails/");
        }
        public INV_PurchaseRequisitionDTO edit(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/edit/");
        }
        public INV_PurchaseRequisitionDTO deactive(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/deactive/");
        }
        public INV_PurchaseRequisitionDTO deactiveM(INV_PurchaseRequisitionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_PurchaseRequisitionFacade/deactiveM/");
        }




    }
}
