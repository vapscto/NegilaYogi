using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_VendorPayment_ReportInterface
    {
        Task<INV_VendorPaymentDTO> getloaddata(INV_VendorPaymentDTO data);
        Task<INV_VendorPaymentDTO> onreport(INV_VendorPaymentDTO data);


    }


}
