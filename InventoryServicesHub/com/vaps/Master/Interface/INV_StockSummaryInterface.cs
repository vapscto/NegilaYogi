using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Master.Interface
{
   public interface INV_StockSummaryInterface
    {
        Task<INV_StockSummaryDTO> getloaddata(INV_StockSummaryDTO data);
        Task<INV_StockSummaryDTO> onreport(INV_StockSummaryDTO data);
        Task<INV_StockSummaryDTO> onreporttwo(INV_StockSummaryDTO data);      
        Task<INV_StockSummaryDTO> onreportthree(INV_StockSummaryDTO data);     
        Task<INV_StockSummaryDTO> load_onchange(INV_StockSummaryDTO data);  
        Task<INV_StockSummaryDTO> getstudent(INV_StockSummaryDTO data);
        Task<INV_StockSummaryDTO> onreportstock(INV_StockSummaryDTO data);
    }
}
