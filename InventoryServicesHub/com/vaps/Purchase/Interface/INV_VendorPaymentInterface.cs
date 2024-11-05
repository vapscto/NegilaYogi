using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_VendorPaymentInterface
    {
        INV_VendorPaymentDTO getloaddata(INV_VendorPaymentDTO data);
        Task<INV_VendorPaymentDTO> getGRNdetail(INV_VendorPaymentDTO data);
        INV_VendorPaymentDTO savedetails(INV_VendorPaymentDTO data);
        INV_VendorPaymentDTO deactive(INV_VendorPaymentDTO data);
        INV_VendorPaymentDTO deactiveGRN(INV_VendorPaymentDTO data);
        INV_VendorPaymentDTO getmodeldetail(INV_VendorPaymentDTO data);

    }


}
