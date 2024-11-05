using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INVMasterCategoryDelegate
    {
        CommonDelegate<INVMasterCategoryDTO, INVMasterCategoryDTO> COMINV = new CommonDelegate<INVMasterCategoryDTO, INVMasterCategoryDTO>();
        public INVMasterCategoryDTO getloaddata(INVMasterCategoryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INVMasterCategoryFacade/getloaddata/");
        }
        public INVMasterCategoryDTO savedetails(INVMasterCategoryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INVMasterCategoryFacade/savedetails/");
        }
      
        public INVMasterCategoryDTO deactive(INVMasterCategoryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INVMasterCategoryFacade/deactive/");
        }
        public INVMasterCategoryDTO getorder(INVMasterCategoryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INVMasterCategoryFacade/getorder/");
        }
         public INVMasterCategoryDTO saveorder(INVMasterCategoryDTO data)
        {
            return COMINV.POSTDataInventory(data, "INVMasterCategoryFacade/saveorder/");
        }
        
    }
}
