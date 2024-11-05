using PreadmissionDTOs.com.vaps.Inventory;
using PreadmissionDTOs.com.vaps.Purchase.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Purchase.Interface
{
    public interface INV_PI_ToSupplierInterface
    {
        INV_PI_ToSupplierDTO getloaddata(INV_PI_ToSupplierDTO data);
        INV_PI_ToSupplierDTO getpiDetail(INV_PI_ToSupplierDTO data);
        Task<INV_PI_ToSupplierDTO> savedetails(INV_PI_ToSupplierDTO data);

        INV_PI_ToSupplierDTO deactive(INV_PI_ToSupplierDTO data);

    }


}
