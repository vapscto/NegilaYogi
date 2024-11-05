using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_T_SalesReturnInterface
    {
        Task<INV_T_SalesReturnDTO> getloaddata(INV_T_SalesReturnDTO data);
        Task<INV_T_SalesReturnDTO> getitem(INV_T_SalesReturnDTO data);
        Task<INV_T_SalesReturnDTO> getitemDetail(INV_T_SalesReturnDTO data);
        Task<INV_T_SalesReturnDTO> savedetails(INV_T_SalesReturnDTO data);
        Task<INV_T_SalesReturnDTO> deactive(INV_T_SalesReturnDTO data);
        Task<INV_T_SalesReturnDTO> viewitem(INV_T_SalesReturnDTO data);
        

    }
}
