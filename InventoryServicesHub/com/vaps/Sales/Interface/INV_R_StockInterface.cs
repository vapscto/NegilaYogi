using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_R_StockInterface
    {
       Task<INV_StockDTO> getloaddata(INV_StockDTO data);
        Task<INV_StockDTO> onreport(INV_StockDTO data);


    }


}
