using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_ItemConsumptionInterface
    {
        INV_ItemConsumptionDTO getloaddata(INV_ItemConsumptionDTO data);      
        INV_ItemConsumptionDTO savedetails(INV_ItemConsumptionDTO data);
        INV_ItemConsumptionDTO deactive(INV_ItemConsumptionDTO data);
        INV_ItemConsumptionDTO deactiveSub(INV_ItemConsumptionDTO data);
        INV_ItemConsumptionDTO getobdetails(INV_ItemConsumptionDTO data);
        Task<INV_ItemConsumptionDTO> getICDetails(INV_ItemConsumptionDTO data);
        INV_ItemConsumptionDTO getsection(INV_ItemConsumptionDTO data);
        INV_ItemConsumptionDTO getstudent(INV_ItemConsumptionDTO data);


        
    }
}
