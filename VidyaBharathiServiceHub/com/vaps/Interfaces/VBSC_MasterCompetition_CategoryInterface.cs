using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VidyaBharathiServiceHub.com.vaps.Interfaces
{
   public interface VBSC_MasterCompetition_CategoryInterface 
    {
        VBSC_MasterCompetition_CategoryDTO loaddata(int dto);
        
        VBSC_MasterCompetition_CategoryDTO savedata(VBSC_MasterCompetition_CategoryDTO data);
        VBSC_MasterCompetition_CategoryDTO Deactivate(VBSC_MasterCompetition_CategoryDTO data);
        VBSC_MasterCompetition_CategoryDTO Organsation(VBSC_MasterCompetition_CategoryDTO data);
        Master_Competition_Category_ClassesDTO savedataCl(Master_Competition_Category_ClassesDTO data);
        Master_Competition_Category_ClassesDTO DeactivateCl(Master_Competition_Category_ClassesDTO data);
        VBSC_Master_Competition_Category_LevelsDTO getdata(int dto);
        VBSC_Master_Competition_Category_LevelsDTO savedataVCl(VBSC_Master_Competition_Category_LevelsDTO data);
        VBSC_Master_Competition_Category_LevelsDTO DeactivateVCl(VBSC_Master_Competition_Category_LevelsDTO data);
    }
}
