using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
    public interface INV_ItemReportInterface
    {
       Task<INV_Master_ItemDTO> getloaddata(INV_Master_ItemDTO data);
        Task<INV_Master_ItemDTO> onreport(INV_Master_ItemDTO data);


    }


}
