using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface MasterSubMenuInterface
    {
        MasterMainMenuDTO MasterSubMenuData(MasterMainMenuDTO mas);

        MasterMainMenuDTO MasterDeleteSubMenuDTO(int ID);

        MasterMainMenuDTO GetSelectedRowDetails(int ID);

        MasterMainMenuDTO GetMasterSubMenuData(MasterMainMenuDTO MasterMainMenuDTO);
    }
}
