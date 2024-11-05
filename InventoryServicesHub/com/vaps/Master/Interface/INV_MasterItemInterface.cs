using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_MasterItemInterface
    {
        INV_Master_ItemDTO getloaddata(INV_Master_ItemDTO data);
        INV_Master_ItemDTO savedetails(INV_Master_ItemDTO data);     
        INV_Master_ItemDTO deactive(INV_Master_ItemDTO data);

        INV_Master_ItemDTO deactiveitax(INV_Master_ItemDTO data);

        
       INV_Master_ItemDTO itemTax(INV_Master_ItemDTO data);
        
    }


}
