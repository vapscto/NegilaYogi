using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_VendorPayment_ReportDelegate
    {
        CommonDelegate<INV_VendorPaymentDTO, INV_VendorPaymentDTO> COMINV = new CommonDelegate<INV_VendorPaymentDTO, INV_VendorPaymentDTO>();
        public INV_VendorPaymentDTO getloaddata(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPayment_ReportFacade/getloaddata/");
        }
        public INV_VendorPaymentDTO onreport(INV_VendorPaymentDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_VendorPayment_ReportFacade/onreport/");
        }

        
    }
}
