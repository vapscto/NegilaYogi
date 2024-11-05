using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_MasterGroupDelegate
    {
        CommonDelegate<INV_Master_GroupDTO, INV_Master_GroupDTO> COMINV = new CommonDelegate<INV_Master_GroupDTO, INV_Master_GroupDTO>();
        public INV_Master_GroupDTO getloaddata(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/getloaddata/");
        }
        public INV_Master_GroupDTO savedetails(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/savedetails/");
        }
        public INV_Master_GroupDTO savedetailsUG(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/savedetailsUG/");
        }
        public INV_Master_GroupDTO savedetailsIG(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/savedetailsIG/");
        }
        public INV_Master_GroupDTO deactive(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/deactive/");
        }
        public INV_Master_GroupDTO groupChange(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/groupChange/");
        }
        public INV_Master_GroupDTO usergroup(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/usergroup/");
        }
        public INV_Master_GroupDTO Itemgroup(INV_Master_GroupDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_MasterGroupFacade/Itemgroup/");
        }
      

        

    }
}
