using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_MasterSupplierInterface
    {
        INV_Master_SupplierDTO getloaddata(INV_Master_SupplierDTO data);
        INV_Master_SupplierDTO savedetails(INV_Master_SupplierDTO data);     
        INV_Master_SupplierDTO deactive(INV_Master_SupplierDTO data);
    }
}
