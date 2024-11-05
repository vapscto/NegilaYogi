using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
  public interface INV_ProductReportInterface
    {

        INV_Master_ProductDTO getalldetails(INV_Master_ProductDTO data);

        INV_Master_ProductDTO getdata(INV_Master_ProductDTO data);
        

        Task<INV_Master_ProductDTO> radiobtndata(INV_Master_ProductDTO data);

    }
}
