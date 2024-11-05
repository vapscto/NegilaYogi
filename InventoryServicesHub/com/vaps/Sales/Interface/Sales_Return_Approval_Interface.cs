using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface Sales_Return_Approval_Interface
    {
        Sales_Return_Apply_DTO getloaddata(Sales_Return_Apply_DTO dto);
        Task<Sales_Return_Apply_DTO> getpidetails(Sales_Return_Apply_DTO dto);
        Sales_Return_Apply_DTO savedetails(Sales_Return_Apply_DTO dto);
    }
}
