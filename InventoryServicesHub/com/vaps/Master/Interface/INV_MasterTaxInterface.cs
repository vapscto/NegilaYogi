using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public  interface INV_MasterTaxInterface
    {
        INV_Master_TaxDTO getloaddata(INV_Master_TaxDTO data);
        INV_Master_TaxDTO savedetails(INV_Master_TaxDTO data);     
        INV_Master_TaxDTO deactive(INV_Master_TaxDTO data);
    }
}
