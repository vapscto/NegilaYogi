using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterUOMDelegate
    {
        CommonDelegate<INV_Master_UOMDTO, INV_Master_UOMDTO> COMINV = new CommonDelegate<INV_Master_UOMDTO, INV_Master_UOMDTO>();
        public INV_Master_UOMDTO getloaddata(INV_Master_UOMDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterUOMFacade/getloaddata/");
        }
        public INV_Master_UOMDTO savedetails(INV_Master_UOMDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterUOMFacade/savedetails/");
        }
      
        public INV_Master_UOMDTO deactive(INV_Master_UOMDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterUOMFacade/deactive/");
        }
        
    }
}
