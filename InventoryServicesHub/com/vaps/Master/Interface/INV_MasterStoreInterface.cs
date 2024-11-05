using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public  interface INV_MasterStoreInterface
    {
        INV_Master_StoreDTO getloaddata(INV_Master_StoreDTO data);
        INV_Master_StoreDTO savedetails(INV_Master_StoreDTO data);     
        INV_Master_StoreDTO deactive(INV_Master_StoreDTO data);
    }
}
