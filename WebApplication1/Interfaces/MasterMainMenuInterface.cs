using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface MasterMainMenuInterface
    {
        MasterMainMenuDTO MasterMainMenuData(MasterMainMenuDTO mas);

        MasterMainMenuDTO MasterDeleteMainMenuDTO(int ID);

        MasterMainMenuDTO GetSelectedRowDetails(int ID);

        MasterMainMenuDTO GetMasterMainMenuData(MasterMainMenuDTO MasterMainMenuDTO);
    }
}
