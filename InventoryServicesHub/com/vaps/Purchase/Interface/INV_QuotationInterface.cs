using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_QuotationInterface
    {
        INV_QuotationDTO getloaddata(INV_QuotationDTO data);
        INV_QuotationDTO getquotationdetails(INV_QuotationDTO data);
        INV_QuotationDTO getpiDetail(INV_QuotationDTO data);
        
        INV_QuotationDTO savedetails(INV_QuotationDTO data);
        INV_QuotationDTO deactiveM(INV_QuotationDTO data);
        INV_QuotationDTO deactive(INV_QuotationDTO data);
     
    }


}
