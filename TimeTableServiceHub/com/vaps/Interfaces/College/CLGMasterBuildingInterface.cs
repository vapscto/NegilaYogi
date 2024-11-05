using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
  public  interface CLGMasterBuildingInterface
    {
        CLGMasterBuilding_DTO getdetails(CLGMasterBuilding_DTO data);
        CLGMasterBuilding_DTO savedetail(CLGMasterBuilding_DTO data);
        CLGMasterBuilding_DTO savedetail1(CLGMasterBuilding_DTO data);
        CLGMasterBuilding_DTO getpagedetails1(int id);
        CLGMasterBuilding_DTO deactive1(CLGMasterBuilding_DTO id);
    }
}
