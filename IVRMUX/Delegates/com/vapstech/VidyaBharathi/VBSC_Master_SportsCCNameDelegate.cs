using CommonLibrary;
using PreadmissionDTOs.com.vaps.VidyaBharathi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VidyaBharathi
{
    public class VBSC_Master_SportsCCNameDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<VBSC_Master_SportsCCNameDTO, VBSC_Master_SportsCCNameDTO> COMMC = new CommonDelegate<VBSC_Master_SportsCCNameDTO, VBSC_Master_SportsCCNameDTO>();
        public VBSC_Master_SportsCCNameDTO getloaddata(VBSC_Master_SportsCCNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCNameFacade/getloaddata/");
        }

        public VBSC_Master_SportsCCNameDTO getInstitute(VBSC_Master_SportsCCNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCNameFacade/getInstitute/");
        }
        

        public VBSC_Master_SportsCCNameDTO savedetails(VBSC_Master_SportsCCNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCNameFacade/savedetails/");
        }
        public VBSC_Master_SportsCCNameDTO deactive(VBSC_Master_SportsCCNameDTO data)
        {
            return COMMC.POSTDataVidyaBharathi(data, "VBSC_Master_SportsCCNameFacade/deactive/");
        }

    }
}
