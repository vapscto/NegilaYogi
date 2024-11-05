using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public  interface INVMasterCategoryInterface
    {
        INVMasterCategoryDTO getloaddata(INVMasterCategoryDTO data);
        INVMasterCategoryDTO savedetails(INVMasterCategoryDTO data);
     
        INVMasterCategoryDTO deactive(INVMasterCategoryDTO data);
        INVMasterCategoryDTO getorder(INVMasterCategoryDTO data);
        INVMasterCategoryDTO saveorder(INVMasterCategoryDTO data);
    }
}
