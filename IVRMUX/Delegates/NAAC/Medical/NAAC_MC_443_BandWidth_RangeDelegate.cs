using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class NAAC_MC_443_BandWidth_RangeDelegate
    {

        CommonDelegate<NAAC_MC_443_BandWidth_Range_DTO, NAAC_MC_443_BandWidth_Range_DTO> comm = new CommonDelegate<NAAC_MC_443_BandWidth_Range_DTO, NAAC_MC_443_BandWidth_Range_DTO>();
        public NAAC_MC_443_BandWidth_Range_DTO loaddata(NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_443_BandWidth_RangeFacade/loaddata");
        }
        public NAAC_MC_443_BandWidth_Range_DTO save(NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_443_BandWidth_RangeFacade/save");
        }
        public NAAC_MC_443_BandWidth_Range_DTO EditData(NAAC_MC_443_BandWidth_Range_DTO data)
        {
            return comm.naacdetailsbypost(data, "NAAC_MC_443_BandWidth_RangeFacade/EditData");
        }
    }
}
