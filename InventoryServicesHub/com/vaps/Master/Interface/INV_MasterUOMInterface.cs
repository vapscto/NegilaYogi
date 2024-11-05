using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public  interface INV_MasterUOMInterface
    {
        INV_Master_UOMDTO getloaddata(INV_Master_UOMDTO data);
        INV_Master_UOMDTO savedetails(INV_Master_UOMDTO data);
     
        INV_Master_UOMDTO deactive(INV_Master_UOMDTO data);
    }
}
