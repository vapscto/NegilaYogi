using CommonLibrary;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Canteen
{
    public class FooditemtaxDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FooditemtaxDTO, FooditemtaxDTO> COMMM = new CommonDelegate<FooditemtaxDTO, FooditemtaxDTO>();

        public FooditemtaxDTO loaddata(FooditemtaxDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditemtaxfacade/loaddata/");
        }
        public FooditemtaxDTO savedata(FooditemtaxDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditemtaxfacade/savedata/");
        }
        public FooditemtaxDTO deactivate(FooditemtaxDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditemtaxfacade/deactivate/");
        }

    }
}
