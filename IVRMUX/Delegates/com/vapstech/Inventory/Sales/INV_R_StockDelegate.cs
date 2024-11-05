using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_R_StockDelegate
    {
        CommonDelegate<INV_StockDTO, INV_StockDTO> COMINV = new CommonDelegate<INV_StockDTO, INV_StockDTO>();
        public INV_StockDTO getloaddata(INV_StockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_StockFacade/getloaddata/");
        }
        public INV_StockDTO onreport(INV_StockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_R_StockFacade/onreport/");
        }

        
    }
}
