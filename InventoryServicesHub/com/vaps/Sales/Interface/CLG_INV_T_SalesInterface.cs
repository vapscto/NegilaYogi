using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface CLG_INV_T_SalesInterface
    {
        INV_T_SalesDTO getloaddata(INV_T_SalesDTO data);
        INV_T_SalesDTO getbranchlist(INV_T_SalesDTO data);
        INV_T_SalesDTO getsemesterlist(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getitem(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getitemDetail(INV_T_SalesDTO data);
        INV_T_SalesDTO savedetails(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getSaletypes(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getSaleItemDetails(INV_T_SalesDTO data);
        INV_T_SalesDTO getSaleItemTax(INV_T_SalesDTO data);
        INV_T_SalesDTO deactive(INV_T_SalesDTO data);
        INV_T_SalesDTO deactiveS(INV_T_SalesDTO data);
        INV_T_SalesDTO deactivetax(INV_T_SalesDTO data);

    }


}
