using CommonLibrary;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Canteen
{
    public class FooditeamDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FooditeamDTO, FooditeamDTO> COMMM = new CommonDelegate<FooditeamDTO, FooditeamDTO>();
      
        public FooditeamDTO loaddata(FooditeamDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditeamfacade/loaddata/");
        }
        public FooditeamDTO savedata(FooditeamDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditeamfacade/savedata/");
        }
        public FooditeamDTO GetEditdata(FooditeamDTO data)//Int32 AMA_Id
        {

            return COMMM.POSTDataByCanteen(data, "Fooditeamfacade/GetEditdata/");
        }
        public FooditeamDTO Getimagedata(FooditeamDTO data)//Int32 AMA_Id
        {

            return COMMM.POSTDataByCanteen(data, "Fooditeamfacade/Getimagedata/");
        }



        
        public FooditeamDTO deactivate(FooditeamDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "Fooditeamfacade/deactivate/");
        }

    }
}
