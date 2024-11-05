using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ConfigurationDelegate
    {
        CommonDelegate<INV_ConfigurationDTO, INV_ConfigurationDTO> COMINV = new CommonDelegate<INV_ConfigurationDTO, INV_ConfigurationDTO>();
        public INV_ConfigurationDTO getloaddata(INV_ConfigurationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ConfigurationFacade/getloaddata/");
        }
        public INV_ConfigurationDTO savedetails(INV_ConfigurationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ConfigurationFacade/savedetails/");
        }    
    }
}
