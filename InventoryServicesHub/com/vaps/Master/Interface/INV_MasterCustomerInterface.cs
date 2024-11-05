using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_MasterCustomerInterface
    {
        INV_Master_CustomerDTO getloaddata(INV_Master_CustomerDTO data);
        INV_Master_CustomerDTO savedetails(INV_Master_CustomerDTO data);     
        INV_Master_CustomerDTO deactive(INV_Master_CustomerDTO data);
    }
}
