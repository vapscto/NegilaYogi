using CommonLibrary;
using PreadmissionDTOs.com.vaps.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Inventory
{
    public class INV_OpeningBalanceDelegate
    {
        CommonDelegate<INV_OpeningBalanceDTO, INV_OpeningBalanceDTO> COMINV = new CommonDelegate<INV_OpeningBalanceDTO, INV_OpeningBalanceDTO>();
        public INV_OpeningBalanceDTO getloaddata(INV_OpeningBalanceDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_OpeningBalanceFacade/getloaddata/");
        }

      
        public INV_OpeningBalanceDTO savedetails(INV_OpeningBalanceDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_OpeningBalanceFacade/savedetails/");
        }
        public INV_OpeningBalanceDTO deactive(INV_OpeningBalanceDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_OpeningBalanceFacade/deactive/");
        }
        public INV_OpeningBalanceDTO getobdetails(INV_OpeningBalanceDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_OpeningBalanceFacade/getobdetails/");
        }
        public INV_OpeningBalanceDTO move_to_stock(INV_OpeningBalanceDTO data)
        {
            return COMINV.POSTDataInventory(data, "INV_OpeningBalanceFacade/move_to_stock/");
        }


        
    }
}
