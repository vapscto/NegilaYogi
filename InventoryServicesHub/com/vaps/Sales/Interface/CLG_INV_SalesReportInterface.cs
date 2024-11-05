using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface CLG_INV_SalesReportInterface
    {
        INV_T_SalesDTO getloaddata(INV_T_SalesDTO data);

        Task<INV_T_SalesDTO> mainradiochange(INV_T_SalesDTO data);
        INV_T_SalesDTO getbranchlist(INV_T_SalesDTO data);
        INV_T_SalesDTO getsemesterlist(INV_T_SalesDTO data);
        Task<INV_T_SalesDTO> getStudentlist(INV_T_SalesDTO data);

        Task<INV_T_SalesDTO> onreport(INV_T_SalesDTO data);


    }


}
