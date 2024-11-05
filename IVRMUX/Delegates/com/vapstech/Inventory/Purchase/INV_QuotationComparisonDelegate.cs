using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_QuotationComparisonDelegate
    {
        CommonDelegate<INV_QuotationDTO, INV_QuotationDTO> COMINV = new CommonDelegate<INV_QuotationDTO, INV_QuotationDTO>();
        public INV_QuotationDTO getloaddata(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationComparisonFacade/getloaddata/");
        }
        public INV_QuotationDTO getpisupplier(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationComparisonFacade/getpisupplier/");
        }
        public INV_QuotationDTO get_Comparison(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationComparisonFacade/get_Comparison/");
        }
        public INV_QuotationDTO getqtdetails(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationComparisonFacade/getqtdetails/");
        }
        public INV_QuotationDTO savedata(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationComparisonFacade/savedata/");
        }

        


    }
}
