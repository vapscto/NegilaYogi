using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Interface
{
    public interface INV_DashboardInterface
    {
        Task<INV_DashboardDTO> getloaddata(INV_DashboardDTO data);
        Task<INV_DashboardDTO> getwarrantydetails(INV_DashboardDTO data);

    }
}
