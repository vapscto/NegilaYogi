using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface InstituteSubMenuInterface
    {
        InstituteMainMenuDTO MasterMainMenuData(InstituteMainMenuDTO mas);

        InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID);

        InstituteMainMenuDTO GetSelectedRowDetails(int ID);
        InstituteMainMenuDTO getMenudetailsByModuleId(InstituteMainMenuDTO ID);

        InstituteMainMenuDTO getSubMenudetailsByMainMenuId(InstituteMainMenuDTO data);

        InstituteMainMenuDTO getmoduledetails(InstituteMainMenuDTO data);
        InstituteMainMenuDTO GetMasterSubMenuData(InstituteMainMenuDTO InstituteMainMenuDTO);

        InstituteMainMenuDTO changeorderData(InstituteMainMenuDTO dto);
    }
}
