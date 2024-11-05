using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_PhyStock_UpdationInterface
    {
        INV_PhyStock_UpdationDTO getloaddata(INV_PhyStock_UpdationDTO data);      
        INV_PhyStock_UpdationDTO savedetails(INV_PhyStock_UpdationDTO data);
        INV_PhyStock_UpdationDTO deactive(INV_PhyStock_UpdationDTO data);
        INV_PhyStock_UpdationDTO getobdetails(INV_PhyStock_UpdationDTO data);


        
    }
}
