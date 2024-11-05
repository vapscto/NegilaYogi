using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ItemConsumptionDelegate
    {
        CommonDelegate<INV_ItemConsumptionDTO, INV_ItemConsumptionDTO> COMINV = new CommonDelegate<INV_ItemConsumptionDTO, INV_ItemConsumptionDTO>();
        public INV_ItemConsumptionDTO getloaddata(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/getloaddata/");
        }


        public INV_ItemConsumptionDTO savedetails(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/savedetails/");
        }
        public INV_ItemConsumptionDTO deactive(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/deactive/");
        }
        public INV_ItemConsumptionDTO deactiveSub(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/deactiveSub/");
        }
        public INV_ItemConsumptionDTO getobdetails(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/getobdetails/");
        }
        public INV_ItemConsumptionDTO getICDetails(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/getICDetails/");
        }
         public INV_ItemConsumptionDTO getsection(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/getsection/");
        }
         public INV_ItemConsumptionDTO getstudent(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionFacade/getstudent/");
        }



    }
}
