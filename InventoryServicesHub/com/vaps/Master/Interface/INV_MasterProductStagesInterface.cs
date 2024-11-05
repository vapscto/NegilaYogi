using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_MasterProductStagesInterface
    {
        INV_Master_ProductDTO getloaddata(INV_Master_ProductDTO data);
        INV_Master_ProductDTO savedetails(INV_Master_ProductDTO data);
        INV_Master_ProductDTO savedetailQty(INV_Master_ProductDTO data); 
        INV_Master_ProductDTO savestoreproduct(INV_Master_ProductDTO data);
        INV_Master_ProductDTO deactive(INV_Master_ProductDTO data);
        INV_Master_ProductDTO deactiveQty(INV_Master_ProductDTO data);
        INV_Master_ProductDTO deactiveptax(INV_Master_ProductDTO data);        
        INV_Master_ProductDTO productTax(INV_Master_ProductDTO data);
        INV_Master_ProductDTO getstages(INV_Master_ProductDTO data);
    }


}
