using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_QuotationDelegate
    {
        CommonDelegate<INV_QuotationDTO, INV_QuotationDTO> COMINV = new CommonDelegate<INV_QuotationDTO, INV_QuotationDTO>();
        public INV_QuotationDTO getloaddata(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/getloaddata/");
        }
        public INV_QuotationDTO getquotationdetails(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/getquotationdetails/");
        }

        public INV_QuotationDTO getpiDetail(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/getpiDetail/");
        }
        public INV_QuotationDTO savedetails(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/savedetails/");
        }

        public INV_QuotationDTO deactive(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/deactive/");
        }

        public INV_QuotationDTO deactiveM(INV_QuotationDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_QuotationFacade/deactiveM/");
        }



    }
}
