using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryServicesHub.com.vaps.Sales.Interface
{
    public interface INV_OpeningBalanceInterface
    {
        INV_OpeningBalanceDTO getloaddata(INV_OpeningBalanceDTO data);      
        INV_OpeningBalanceDTO savedetails(INV_OpeningBalanceDTO data);
        INV_OpeningBalanceDTO deactive(INV_OpeningBalanceDTO data);
        INV_OpeningBalanceDTO getobdetails(INV_OpeningBalanceDTO data);
        INV_OpeningBalanceDTO move_to_stock(INV_OpeningBalanceDTO data);


        
    }
}
