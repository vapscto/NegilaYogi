using System;
using PreadmissionDTOs;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class GeneralConfigDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        CommonDelegate<GeneralConfigDTO, GeneralConfigDTO> COMMM = new CommonDelegate<GeneralConfigDTO, GeneralConfigDTO>();

        CommonDelegate<CommonDTO, CommonDTO> COMMMM = new CommonDelegate<CommonDTO, CommonDTO>();

        public GeneralConfigDelegate() { }

        public GeneralConfigDelegate(FacadeUrl config) { _config = config; fdu = config; }
        public GeneralConfigDTO savegenConfigData(GeneralConfigDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "GenConfigFacade/savegenConfigData/");
        }
        public GeneralConfigDTO getcontent(GeneralConfigDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "GenConfigFacade/getcontent/");
        }
        public GeneralConfigDTO Configurationget(GeneralConfigDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "GenConfigFacade/Configurationget/");
        }
        public GeneralConfigDTO geteditdata(GeneralConfigDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "GenConfigFacade/geteditdata/");
        }
        public GeneralConfigDTO deleteUserNameconfig(GeneralConfigDTO mstConfig)
        {
            return COMMM.POSTData(mstConfig, "GenConfigFacade/deleteUserNameconfig/");
        }
    }
}
