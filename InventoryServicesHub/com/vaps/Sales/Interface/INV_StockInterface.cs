using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_StockInterface
    {
        INV_StockDTO getloaddata(INV_StockDTO data);       
        INV_StockDTO savedetails(INV_StockDTO data);
        INV_StockDTO editStock(INV_StockDTO data);

        
    }
}
