using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class ExpirySettingsDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<ExpirySettingsDTO, ExpirySettingsDTO> _areazone = new CommonDelegate<ExpirySettingsDTO, ExpirySettingsDTO>();

        public ExpirySettingsDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "ExpirySettingsFacade/getdata/");
        }
        public ExpirySettingsDTO savedata(ExpirySettingsDTO data)
        {
            return _areazone.POSTDataTransport(data, "ExpirySettingsFacade/savedata/");
        }
        public ExpirySettingsDTO getdatadetails(ExpirySettingsDTO data)
        {
            return _areazone.POSTDataTransport(data, "ExpirySettingsFacade/getdatadetails/");
        }

        
    }

}
