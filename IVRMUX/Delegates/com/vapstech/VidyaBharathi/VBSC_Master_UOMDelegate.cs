using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Master_UOMDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VBSC_Master_UOMDTO, VBSC_Master_UOMDTO> COMMC = new CommonDelegate<VBSC_Master_UOMDTO, VBSC_Master_UOMDTO>();
        public VBSC_Master_UOMDTO loaddata(int id)
        {
            return COMMC.GetDataByVidyaBharathi(id, "VBSC_Master_UOMFacade/loaddata/");
        }
        
        public VBSC_Master_UOMDTO savedetails(VBSC_Master_UOMDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_UOMFacade/savedata/");
        }

        public VBSC_Master_UOMDTO deactive(VBSC_Master_UOMDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_UOMFacade/deactive/");
        }

        //competition level

        public VBSC_Master_UOMDTO getloaddatalevel(int id)
        {
            return COMMC.GetDataByVidyaBharathi(id, "VBSC_Master_UOMFacade/getloaddatalevel/");
        }

        public VBSC_Master_UOMDTO savedetailslevel(VBSC_Master_UOMDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_UOMFacade/savedatalevel/");
        }
        public VBSC_Master_UOMDTO deactivelevel(VBSC_Master_UOMDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_UOMFacade/deactivelevel/");
        }

    }
}
