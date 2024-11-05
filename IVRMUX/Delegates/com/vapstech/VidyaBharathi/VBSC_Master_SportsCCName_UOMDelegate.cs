using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Master_SportsCCName_UOMDelegate
    {
        CommonDelegate<VBSC_Master_SportsCCName_UOMDTO, VBSC_Master_SportsCCName_UOMDTO> COMSPRT = new CommonDelegate<VBSC_Master_SportsCCName_UOMDTO, VBSC_Master_SportsCCName_UOMDTO>();

        public VBSC_Master_SportsCCName_UOMDTO getDetails(VBSC_Master_SportsCCName_UOMDTO data)
        {
            return COMSPRT.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCName_UOMFacade/getDetails/");
        }

        public VBSC_Master_SportsCCName_UOMDTO save(VBSC_Master_SportsCCName_UOMDTO obj)
        {
            return COMSPRT.POSTDataVidyaBharathi(obj, "VBSC_Master_SportsCCName_UOMFacade/save/");
        }

        public VBSC_Master_SportsCCName_UOMDTO EditDetails(int id)
        {
            return COMSPRT.GetDataByVidyaBharathi(id, "VBSC_Master_SportsCCName_UOMFacade/EditDetails/");
        }

        public VBSC_Master_SportsCCName_UOMDTO deactivate(VBSC_Master_SportsCCName_UOMDTO obj)
        {
            return COMSPRT.POSTDataVidyaBharathi(obj, "VBSC_Master_SportsCCName_UOMFacade/deactivate/");
        }
    }
}

