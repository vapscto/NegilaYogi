using CommonLibrary;
using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Sports
{
    public class MasterCompetitionCategoryDelegate
    {
        CommonDelegate<MasterCompetitionCategoryDTO, MasterCompetitionCategoryDTO> COMSPRT = new CommonDelegate<MasterCompetitionCategoryDTO, MasterCompetitionCategoryDTO>();

        public MasterCompetitionCategoryDTO getDetails(MasterCompetitionCategoryDTO data)
        {
            return COMSPRT.POSTDataSports(data, "MasterCompetitionCategoryFacade/getDetails/");
        }
        public MasterCompetitionCategoryDTO save(MasterCompetitionCategoryDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterCompetitionCategoryFacade/save/");
        }
        public MasterCompetitionCategoryDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByIdSports(id, "MasterCompetitionCategoryFacade/EditDetails/");
        }
        public MasterCompetitionCategoryDTO deactivate(MasterCompetitionCategoryDTO obj)
        {
            return COMSPRT.POSTDataSports(obj, "MasterCompetitionCategoryFacade/deactivate/");
        }
    }
}
