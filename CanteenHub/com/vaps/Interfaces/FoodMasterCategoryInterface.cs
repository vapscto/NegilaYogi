using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Interfaces
{
   public interface FoodMasterCategoryInterface
    {
        FoodMasterCategoryDTO loaddata(FoodMasterCategoryDTO data);
        FoodMasterCategoryDTO savedata(FoodMasterCategoryDTO data);
        FoodMasterCategoryDTO GetEditdata(FoodMasterCategoryDTO data);
        FoodMasterCategoryDTO deactivate(FoodMasterCategoryDTO data);

    }
}
