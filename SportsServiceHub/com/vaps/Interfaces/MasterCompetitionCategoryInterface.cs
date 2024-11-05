using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface MasterCompetitionCategoryInterface
    {
        MasterCompetitionCategoryDTO getDetails(MasterCompetitionCategoryDTO data);
        MasterCompetitionCategoryDTO saveRecord(MasterCompetitionCategoryDTO obj);
        MasterCompetitionCategoryDTO EditDetails(int id);
        MasterCompetitionCategoryDTO deactivate(MasterCompetitionCategoryDTO obj);
    }
}
