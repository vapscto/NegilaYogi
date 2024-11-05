using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_ConfigurationInterface
    {
        Task<INV_ConfigurationDTO> getloaddata(INV_ConfigurationDTO data);
        INV_ConfigurationDTO savedetails(INV_ConfigurationDTO data);                  
    }
}
