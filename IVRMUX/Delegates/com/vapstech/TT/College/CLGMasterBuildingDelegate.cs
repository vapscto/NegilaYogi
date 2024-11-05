using CommonLibrary;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT.College
{
    public class CLGMasterBuildingDelegate
    {
        CommonDelegate<CLGMasterBuilding_DTO, CLGMasterBuilding_DTO> comm = new CommonDelegate<CLGMasterBuilding_DTO, CLGMasterBuilding_DTO>();

      public CLGMasterBuilding_DTO getdetails(CLGMasterBuilding_DTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGMasterBuildingFacade/getdetails");
        }
        public CLGMasterBuilding_DTO savedetail(CLGMasterBuilding_DTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGMasterBuildingFacade/savedetail");
        }
public CLGMasterBuilding_DTO savedetail1(CLGMasterBuilding_DTO data)
        {
            return comm.POSTDataTimeTable(data, "CLGMasterBuildingFacade/savedetail1");
        }
        public CLGMasterBuilding_DTO getpagedetails1(int id)
        {
            return comm.GetDataByIdTimeTable(id, "CLGMasterBuildingFacade/getpagedetails1");
            
        }
        public CLGMasterBuilding_DTO deactive1(CLGMasterBuilding_DTO id)
        {
            return comm.POSTDataTimeTable(id, "CLGMasterBuildingFacade/deactive1");
        }
    }
}
