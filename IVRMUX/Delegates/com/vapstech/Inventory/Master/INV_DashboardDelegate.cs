using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_DashboardDelegate
    {
        CommonDelegate<INV_DashboardDTO, INV_DashboardDTO> COMINV = new CommonDelegate<INV_DashboardDTO, INV_DashboardDTO>();
        public INV_DashboardDTO getloaddata(INV_DashboardDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DashboardFacade/getloaddata/");
        }
       public INV_DashboardDTO getwarrantydetails(INV_DashboardDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_DashboardFacade/getwarrantydetails/");
        }
      
        
    }
}
