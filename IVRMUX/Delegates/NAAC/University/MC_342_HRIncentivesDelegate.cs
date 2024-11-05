using CommonLibrary;
using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.University
{
    public class MC_342_HRIncentivesDelegate
    {
        CommonDelegate<MC_342_HRIncentivesDTO, MC_342_HRIncentivesDTO> comm = new CommonDelegate<MC_342_HRIncentivesDTO, MC_342_HRIncentivesDTO>();
        public MC_342_HRIncentivesDTO loaddata(MC_342_HRIncentivesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_342_HRIncentivesFacade/getdata");
        }
        public MC_342_HRIncentivesDTO savedata(MC_342_HRIncentivesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_342_HRIncentivesFacade/savedata");
        }
        public MC_342_HRIncentivesDTO deactive(MC_342_HRIncentivesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_342_HRIncentivesFacade/deactive");
        }
        public MC_342_HRIncentivesDTO editdata(MC_342_HRIncentivesDTO data)
        {
            return comm.naacdetailsbypost(data, "MC_342_HRIncentivesFacade/editdata");
        }
    }
}
