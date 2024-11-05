using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_VendorPaymentDelegate
    {
        CommonDelegate<INV_VendorPaymentDTO, INV_VendorPaymentDTO> COMINV = new CommonDelegate<INV_VendorPaymentDTO, INV_VendorPaymentDTO>();
        public INV_VendorPaymentDTO getloaddata(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/getloaddata/");
        }
        public INV_VendorPaymentDTO getGRNdetail(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/getGRNdetail/");
        }
        public INV_VendorPaymentDTO savedetails(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/savedetails/");
        }
   
        public INV_VendorPaymentDTO deactive(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/deactive/");
        }
        public INV_VendorPaymentDTO deactiveGRN(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/deactiveGRN/");
        }
        public INV_VendorPaymentDTO getmodeldetail(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPaymentFacade/getmodeldetail/");
        }
     

        

    }
}
