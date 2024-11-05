using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterItemDelegate
    {
        CommonDelegate<INV_Master_ItemDTO, INV_Master_ItemDTO> COMINV = new CommonDelegate<INV_Master_ItemDTO, INV_Master_ItemDTO>();
        public INV_Master_ItemDTO getloaddata(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterItemFacade/getloaddata/");
        }
        public INV_Master_ItemDTO savedetails(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterItemFacade/savedetails/");
        }
        public INV_Master_ItemDTO deactive(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterItemFacade/deactive/");
        }
        public INV_Master_ItemDTO deactiveitax(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterItemFacade/deactiveitax/");
        }
        
        public INV_Master_ItemDTO itemTax(INV_Master_ItemDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterItemFacade/itemTax/");
        }
        
    }
}
