using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Transport;

namespace corewebapi18072016.Delegates.com.vapstech.Transport
{
    public class LocationFeeGroupMappingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";

        CommonDelegate<TR_Location_FeeGroup_MappingDTO, TR_Location_FeeGroup_MappingDTO> _areazone = new CommonDelegate<TR_Location_FeeGroup_MappingDTO, TR_Location_FeeGroup_MappingDTO>();
        CommonDelegate<TR_Location_AmountDTO, TR_Location_AmountDTO> _area = new CommonDelegate<TR_Location_AmountDTO, TR_Location_AmountDTO>();

        public TR_Location_FeeGroup_MappingDTO getdata(int id)
        {
            return _areazone.GetDataByIdTransport(id, "LocationFeeGroupMappingFacade/getdata/");
        }
        public TR_Location_FeeGroup_MappingDTO savedata(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/savedata/");
        }
        public TR_Location_FeeGroup_MappingDTO geteditdata(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/geteditdata/");
        }
        public TR_Location_FeeGroup_MappingDTO activedeactive(TR_Location_FeeGroup_MappingDTO data)
        {
            return _areazone.POSTDataTransport(data, "LocationFeeGroupMappingFacade/activedeactive/");
        }

        public TR_Location_AmountDTO savedataamount(TR_Location_AmountDTO data)
        {
            return _area.POSTDataTransport(data, "LocationFeeGroupMappingFacade/savedataamount/");
        }
        public TR_Location_AmountDTO geteditdataamount(TR_Location_AmountDTO data)
        {
            return _area.POSTDataTransport(data, "LocationFeeGroupMappingFacade/geteditdataamount/");
        }
        public TR_Location_AmountDTO activedeactiveamount(TR_Location_AmountDTO data)
        {
            return _area.POSTDataTransport(data, "LocationFeeGroupMappingFacade/activedeactiveamount/");
        }
    }

}
