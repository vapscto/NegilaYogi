using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_Quotation_ReportInterface
    {
       Task<INV_QuotationDTO> getloaddata(INV_QuotationDTO data);
        Task<INV_QuotationDTO> onreport(INV_QuotationDTO data);

    }


}
