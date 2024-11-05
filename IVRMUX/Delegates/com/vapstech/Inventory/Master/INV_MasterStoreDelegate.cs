using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterStoreDelegate
    {
        CommonDelegate<INV_Master_StoreDTO, INV_Master_StoreDTO> COMINV = new CommonDelegate<INV_Master_StoreDTO, INV_Master_StoreDTO>();
        public INV_Master_StoreDTO getloaddata(INV_Master_StoreDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterStoreFacade/getloaddata/");
        }
        public INV_Master_StoreDTO savedetails(INV_Master_StoreDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterStoreFacade/savedetails/");
        }
        public INV_Master_StoreDTO deactive(INV_Master_StoreDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterStoreFacade/deactive/");
        }

    }
}
