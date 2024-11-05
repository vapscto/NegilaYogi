using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public  interface INV_MasterGroupInterface
    {
        INV_Master_GroupDTO getloaddata(INV_Master_GroupDTO data);
        INV_Master_GroupDTO savedetails(INV_Master_GroupDTO data);
        INV_Master_GroupDTO savedetailsUG(INV_Master_GroupDTO data);
        INV_Master_GroupDTO savedetailsIG(INV_Master_GroupDTO data);
        INV_Master_GroupDTO deactive(INV_Master_GroupDTO data);
        INV_Master_GroupDTO groupChange(INV_Master_GroupDTO data);
        INV_Master_GroupDTO usergroup(INV_Master_GroupDTO data);
        INV_Master_GroupDTO Itemgroup(INV_Master_GroupDTO data);
     
        
    }
}
