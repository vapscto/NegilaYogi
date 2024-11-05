using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_ItemConsumptionReportDelegate
    {
        CommonDelegate<INV_ItemConsumptionDTO, INV_ItemConsumptionDTO> COMINV = new CommonDelegate<INV_ItemConsumptionDTO, INV_ItemConsumptionDTO>();
        public INV_ItemConsumptionDTO getloaddata(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionReportFacade/getloaddata/");
        }
        public INV_ItemConsumptionDTO onreport(INV_ItemConsumptionDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_ItemConsumptionReportFacade/onreport/");
        }

        
    }
}
