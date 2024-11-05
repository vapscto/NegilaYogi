using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Interfaces
{
    public interface FooditeamInterface
    {
        //FooditeamDTO loaddata(int id);
        FooditeamDTO loaddata(FooditeamDTO data);
        FooditeamDTO savedata(FooditeamDTO data);
        FooditeamDTO GetEditdata(FooditeamDTO data);
        FooditeamDTO Getimagedata(FooditeamDTO data);
        FooditeamDTO deactivate(FooditeamDTO data);
        FooditeamDTO Createpin(FooditeamDTO data);
        FooditeamDTO changepassword(FooditeamDTO data);
        FooditeamDTO Forgotpin(FooditeamDTO data);


    }
}
