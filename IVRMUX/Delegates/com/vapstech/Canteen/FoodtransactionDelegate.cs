using CommonLibrary;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Canteen
{
    public class FoodtransactionDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FoodtransactionDTO, FoodtransactionDTO> COMMM = new CommonDelegate<FoodtransactionDTO, FoodtransactionDTO>();

        public FoodtransactionDTO loaddata(FoodtransactionDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Foodtransactionfacade/loaddata/");
        }
         public FoodtransactionDTO FoodItem(FoodtransactionDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Foodtransactionfacade/FoodItem/");
        }
        public FoodtransactionDTO FoodItemtax(FoodtransactionDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Foodtransactionfacade/FoodItemtax/");
        }
        public FoodtransactionDTO savedata(FoodtransactionDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Foodtransactionfacade/savedata/");
        }
        public FoodtransactionDTO deactivate(FoodtransactionDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Foodtransactionfacade/deactivate/");
        }

    }
}
