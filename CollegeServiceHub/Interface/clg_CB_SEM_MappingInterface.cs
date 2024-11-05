using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
   public  interface clg_CB_SEM_MappingInterface
    {
        clg_CB_SEM_MappingDTO GetDropDownList(clg_CB_SEM_MappingDTO scAllt);

        
        clg_CB_SEM_MappingDTO Getbranch(clg_CB_SEM_MappingDTO data);
        clg_CB_SEM_MappingDTO savesem(clg_CB_SEM_MappingDTO data);

        clg_CB_SEM_MappingDTO Editrecord(clg_CB_SEM_MappingDTO data);

        clg_CB_SEM_MappingDTO sempopup(clg_CB_SEM_MappingDTO data);
        clg_CB_SEM_MappingDTO deactivate(clg_CB_SEM_MappingDTO data);

        

    }
}
