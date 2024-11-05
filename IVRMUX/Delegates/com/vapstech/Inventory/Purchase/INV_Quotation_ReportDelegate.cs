using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_Quotation_ReportDelegate
    {
        CommonDelegate<INV_QuotationDTO, INV_QuotationDTO> COMINV = new CommonDelegate<INV_QuotationDTO, INV_QuotationDTO>();
        public INV_QuotationDTO getloaddata(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Quotation_ReportFacade/getloaddata/");
        }
        public INV_QuotationDTO onreport(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_Quotation_ReportFacade/onreport/");
        }

        
    }
}
