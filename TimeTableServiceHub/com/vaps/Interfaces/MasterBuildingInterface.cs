using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface MasterBuildingInterface
    {
        TT_Master_BuildingDTO savedetail(TT_Master_BuildingDTO objcategory);
        TT_Master_BuildingDTO savedetail1(TT_Master_BuildingDTO objcategory);
        TT_Master_BuildingDTO getdetails(int id);
        TT_Master_BuildingDTO getpagedetails(long id);
        TT_Master_BuildingDTO getpagedetails1(long id);
        TT_Master_BuildingDTO deactivate(TT_Master_BuildingDTO id);
        TT_Master_BuildingDTO deactivate1(TT_Master_BuildingDTO id);

    }
}
