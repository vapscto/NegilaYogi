using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_StockDelegate
    {
        CommonDelegate<INV_StockDTO, INV_StockDTO> COMINV = new CommonDelegate<INV_StockDTO, INV_StockDTO>();
        public INV_StockDTO getloaddata(INV_StockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockFacade/getloaddata/");
        }
      
        public INV_StockDTO savedetails(INV_StockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockFacade/savedetails/");
        }
        public INV_StockDTO editStock(INV_StockDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_StockFacade/editStock/");
        }

        
    }
}
