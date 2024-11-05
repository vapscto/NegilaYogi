using CommonLibrary;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Canteen
{
    public class FoodMasterCategoryDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<FoodMasterCategoryDTO, FoodMasterCategoryDTO> COMMM = new CommonDelegate<FoodMasterCategoryDTO, FoodMasterCategoryDTO>();

        public FoodMasterCategoryDTO loaddata(FoodMasterCategoryDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "FoodMasterCategoryFacade/loaddata/");
        }
        public FoodMasterCategoryDTO savedata(FoodMasterCategoryDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "FoodMasterCategoryFacade/savedata/");
        }
        public FoodMasterCategoryDTO GetEditdata(FoodMasterCategoryDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "FoodMasterCategoryFacade/GetEditdata/");
        }

        public FoodMasterCategoryDTO deactivate(FoodMasterCategoryDTO data)
        {
            return COMMM.POSTDataByCanteen(data, "FoodMasterCategoryFacade/deactivate/");
        }
    }
}
