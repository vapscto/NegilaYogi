using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_QuotationComparisonInterface
    {
        INV_QuotationDTO getloaddata(INV_QuotationDTO data);
        INV_QuotationDTO getpisupplier(INV_QuotationDTO data);
        INV_QuotationDTO get_Comparison(INV_QuotationDTO data);
        INV_QuotationDTO getqtdetails(INV_QuotationDTO data);

        INV_QuotationDTO savedata(INV_QuotationDTO data);


        
    }


}
