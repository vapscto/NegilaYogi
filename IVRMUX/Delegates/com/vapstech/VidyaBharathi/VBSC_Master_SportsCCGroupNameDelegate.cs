using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Master_SportsCCGroupNameDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VBSC_Master_SportsCCGroupNameDTO, VBSC_Master_SportsCCGroupNameDTO> COMMC = new CommonDelegate<VBSC_Master_SportsCCGroupNameDTO, VBSC_Master_SportsCCGroupNameDTO>();
        public VBSC_Master_SportsCCGroupNameDTO getloaddata(VBSC_Master_SportsCCGroupNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCGroupNameFacade/getloaddata/");
        }
        public VBSC_Master_SportsCCGroupNameDTO savedetails(VBSC_Master_SportsCCGroupNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCGroupNameFacade/savedetails/");
        }
        public VBSC_Master_SportsCCGroupNameDTO deactive(VBSC_Master_SportsCCGroupNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCGroupNameFacade/deactive/");
        }

    }
}
